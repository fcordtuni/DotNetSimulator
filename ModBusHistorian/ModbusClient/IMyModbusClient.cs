namespace ModBusHistorian.ModbusClient;

// ReSharper disable UnusedMemberInSuper.Global
/// <summary>
/// Interface for the modbus client
/// </summary>
public interface IMyModbusClient
{
    /// <summary>
    /// Time in ms between two polls
    /// </summary>
    int PollingTimeMs { get; set; }
    
    /// <summary>
    /// Time in ms between two reconnects
    /// </summary>
    int ReconnectTimeMs { get; set; }
    
    /// <summary>
    /// When true, the client tries to reconnect automatically
    /// </summary>
    bool IsAutoReconnectEnabled { get; set; }
    
    /// <summary>
    /// IP address of the modbus server
    /// </summary>
    string IpAddress { get; set; }
    
    /// <summary>
    /// Port of the modbus server
    /// </summary>
    int Port { get; set; }
    
    /// <summary>
    /// Connects to the modbus server
    /// </summary>
    Task Connect(CancellationToken cancellationToken);
    
    /// <summary>
    /// Disconnects from the modbus server
    /// </summary>
    Task Disconnect();

    /// <summary>
    /// Defines an address range that gets polled as MODBUS InputRegister 
    /// </summary>
    /// <param name="addressRange">The address range polled for</param>
    void AddReadInputRegisterPolling(AddressRange addressRange);

    // ReSharper disable once UnusedMember.Global
    /// <summary>
    /// Returns the value of an address
    /// </summary>
    /// <param name="address">The address (needs to be defined before for polling)</param>
    /// <returns></returns>
    int? GetInputRegisterValue(int address);

    short[] GetInputRegisterValues(int from, int to);

    void WriteCoil(int address, bool value);
    
    // ReSharper disable once UnusedMember.Global
    void Dispose();
}