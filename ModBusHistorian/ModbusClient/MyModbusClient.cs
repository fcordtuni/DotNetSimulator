﻿//Author: FCORDT

namespace ModBusHistorian.ModbusClient;

public class MyModbusClient : IMyModbusClient
{
    public MyModbusClient(IConfiguration iConfig)
    {
        IpAddress = iConfig.GetSection("ModbusHistorian").GetSection("ModbusServer").GetValue("IpAdress", "127.0.0.1") ?? "127.0.0.1";
        Port = iConfig.GetSection("ModbusHistorian").GetSection("ModbusServer").GetValue("Port", 502);
        PollingTimeMs = iConfig.GetSection("ModbusHistorian").GetValue("PollingTimeMs", 1000);
        ReconnectTimeMs = iConfig.GetSection("ModbusHistorian").GetValue("ReconnectTimeMs", 1000);
        IsAutoReconnectEnabled = iConfig.GetSection("ModbusHistorian").GetValue("AutoReconnect", true);
        _inputRegisters = new Dictionary<int, int>();
        _pollingTasks = new List<Task>();
        _client = new EasyModbus.ModbusClient();
        _taskTokenSource = new CancellationTokenSource();
    }

    public int PollingTimeMs { get; set; }
    public int ReconnectTimeMs { get; set; }
    public bool IsAutoReconnectEnabled { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }

    private readonly IList<Task> _pollingTasks;
    private readonly IDictionary<int, int> _inputRegisters;

    private readonly EasyModbus.ModbusClient _client;
    private readonly CancellationTokenSource _taskTokenSource;
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