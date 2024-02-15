//Author: FCORDT

namespace ModbusDeviceLibrary.Modbus;

/// <summary>
/// A interface to map <see cref="IModbusDevice"/> to Coils and so on
/// </summary>
public interface IModbusMapper
{
    /// <summary>
    /// Tells the IModbusMapper to map a number of Coils for the given caller
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="descriptors"></param>
    void RegisterCoils(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors);

    /// <summary>
    /// Gets the Coils mapped to the caller
    /// </summary>
    /// <param name="caller"></param>
    /// <returns></returns>
    Span<bool> GetCoils(IModbusDevice caller);

    // ReSharper disable once UnusedMember.Global
    /// <summary>
    /// Tells the IModbusMapper to map a number of Discrete Inputs for the given caller
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="descriptors"></param>
    void RegisterDiscreteInputs(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors);

    // ReSharper disable once UnusedMember.Global
    /// <summary>
    /// Gets the Discrete Inputs mapped to the caller
    /// </summary>
    /// <param name="caller"></param>
    /// <returns></returns>
    ReadOnlySpan<bool> GetDiscreteInputs(IModbusDevice caller);

    /// <summary>
    /// Tells the IModbusMapper to map a number of Holding Registers for the given caller
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="descriptor"></param>
    void RegisterHoldingRegisters(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptor);

    /// <summary>
    /// Gets the Holding Registers mapped to the caller
    /// </summary>
    /// <param name="caller"></param>
    /// <returns></returns>
    Span<short> GetHoldingRegisters(IModbusDevice caller);

    // ReSharper disable once UnusedMember.Global
    /// <summary>
    /// Tells the IModbusMapper to map a number of Input Registers for the given caller
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="descriptors"></param>
    void RegisterInputRegisters(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors);

    // ReSharper disable once UnusedMember.Global
    /// <summary>
    /// Gets the Input Registers mapped to the caller
    /// </summary>
    /// <param name="caller"></param>
    /// <returns></returns>
    ReadOnlySpan<short> GetInputRegisters(IModbusDevice caller);
}
