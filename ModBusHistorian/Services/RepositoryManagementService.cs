//Author: fcordt

using ModbusDeviceLibrary.Modbus;
using ModBusHistorian.ModbusClient;
using ModBusHistorian.Models;
using ModBusHistorian.Repositories;
using NLog;

namespace ModBusHistorian.Services;

/// <summary>
/// This is a hosted service that initializes the repository service and adds a data series recorder to it.
/// </summary>
internal class RepositoryManagementService(
    IMyModbusClient myModbusClient,
    IDataSeriesRepository repositoryService,
    IConfiguration iConfig)
    : IHostedService, IDisposable
{
    private readonly List<DataSeriesRecorder<double?>> _recorders = [];
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Connects to modbus server
        try
        {
            Logger.Info("Startup: Connecting to MODBUS server and configuring it.");

            await myModbusClient.Connect(cancellationToken);

            Logger.Info("Startup: Initializing data series recorder.");
            var coils = new HashSet<int>();
            int start = int.MaxValue, end = 0;
            foreach (var deviceInfo in iConfig.GetSection("ModbusHistorian")
                         .GetSection("ModbusServer").GetSection("Devices").GetChildren())
            {
                var from = deviceInfo.GetValue("from", -1);
                var to = deviceInfo.GetValue("to", -1);
                var frequencySec = deviceInfo.GetValue("refreshPeriodS", 10);
                if (from < 0)
                {
                    throw MissingConfigurationException.Create(deviceInfo, "from");
                }

                if (to < 0)
                {
                    throw MissingConfigurationException.Create(deviceInfo, "to");
                }

                if (frequencySec <= 0)
                {
                    throw new ArgumentException($"refreshPeriodS <= 0 in configuration {deviceInfo.Path}");
                }

                if (to <= from)
                {
                    throw new ArgumentException($"to <= from in configuration {deviceInfo.Path}");
                }

                start = Math.Min(start, from);
                end = Math.Max(end, to);
                _recorders.Add(new DataSeriesRecorder<double?>(
                    repositoryService, 
                    new TimeSpan(0,0,frequencySec),
                    () =>
                    {
                        
                        var value = myModbusClient.GetInputRegisterValues(from, to);
                        return ModbusUtils.ReadHoldingRegisterInt(value);
                    }, new Reference(deviceInfo.Key))
                );
                if (deviceInfo.GetSection("enableCoil").Exists())
                {
                    coils.Add(deviceInfo.GetValue("enableCoil", 0));
                }
            }
            myModbusClient.AddReadInputRegisterPolling(new AddressRange(start, end - Math.Min(start, end)));
            // This call "starts" the production for simulation purposes
            Logger.Info("Startup: Starting production for simulation purposes.");
            foreach(var coil in coils)
            {
                myModbusClient.WriteCoil(coil, true);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            Logger.Info("Shutdown: Stopping recorders");
            foreach (var dataSeriesRecorder in _recorders)
            {
                dataSeriesRecorder.Dispose();
            }
            
            Logger.Info("Shutdown: Disconnecting from MODBUS server.");
            myModbusClient.Disconnect();
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        repositoryService.Dispose();
    }
}