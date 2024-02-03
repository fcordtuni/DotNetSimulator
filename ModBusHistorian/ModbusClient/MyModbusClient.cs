//Author: FCORDT

namespace ModBusHistorian.ModbusClient;

public class MyModbusClient(IConfiguration iConfig) : IMyModbusClient
{
    private readonly List<Task> _pollingTasks = [];
    private readonly Dictionary<int, int> _inputRegisters = new();

    private readonly EasyModbus.ModbusClient _client = new();
    private readonly CancellationTokenSource _taskTokenSource = new();
    public int PollingTimeMs { get; set; } = iConfig.GetSection("ModbusHistorian").GetValue("PollingTimeMs", 1000);
    public int ReconnectTimeMs { get; set; } = iConfig.GetSection("ModbusHistorian").GetValue("ReconnectTimeMs", 1000);
    public bool IsAutoReconnectEnabled { get; set; } = iConfig.GetSection("ModbusHistorian").GetValue("AutoReconnect", true);
    public string IpAddress { get; set; } = iConfig.GetSection("ModbusHistorian").GetSection("ModbusServer").GetValue("IpAdress", "127.0.0.1") ?? "127.0.0.1";
    public int Port { get; set; } = iConfig.GetSection("ModbusHistorian").GetSection("ModbusServer").GetValue("Port", 502);

    public Task Connect(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            _client.Connect(IpAddress, Port);
            while (!_client.Connected && IsAutoReconnectEnabled)
            {
                Task.Delay(ReconnectTimeMs, cancellationToken);
                _client.Connect(IpAddress, Port);
            }
        }, cancellationToken);
    }

    public Task Disconnect()
    {
        _taskTokenSource.Cancel();
        return Task.WhenAll(_pollingTasks).ContinueWith(delegate { _client.Disconnect(); });
    }

    private Task CreatePollingTask(AddressRange range, CancellationToken taskToken)
    {
        return Task.Run(() =>
        {
            if (_client.Connected)
            {
                var registers = _client.ReadHoldingRegisters(range.StartingAddress, range.Quantity);
                for (var i = 0; i < registers.Length; i++)
                {
                    _inputRegisters[range.StartingAddress + i] = registers[i];
                }
            }

            Task.Delay(PollingTimeMs, taskToken);
        }, taskToken);
    }

    public void AddReadInputRegisterPolling(AddressRange addressRange)
    {
        _pollingTasks.Add(CreatePollingTask(addressRange, _taskTokenSource.Token));
    }

    public int? GetInputRegisterValue(int address)
    {
        if (!_inputRegisters.TryGetValue(address, out var value))
        {
            return null;
        }
        return value;
    }

    public short[] GetInputRegisterValues(int from, int to)
    {
        var rVal = new short[to - from];
        for (var i = 0; i < rVal.Length; i++)
        {
            _inputRegisters.TryGetValue(i + from, out var val);
            rVal[i] = (short)val;
        }

        return rVal;
    }

    public void WriteCoil(int address, bool value)
    {
        if (_client.Connected)
        {
            _client.WriteSingleCoil(address, value);
        }
    }

    public void Dispose()
    {
        _taskTokenSource.Dispose();
    }
}