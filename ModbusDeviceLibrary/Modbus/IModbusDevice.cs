//Author: FCORDT

namespace ModbusDeviceLibrary.Modbus;

/// <summary>
/// defines a Modbus Device that can use a <see cref="IModbusMapper"/> to provide and Read Modbus Data
/// </summary>
public interface IModbusDevice
{
    // ReSharper disable once UnusedMemberInSuper.Global
    /// <summary>
    /// Registers a <see cref="IModbusMapper"/> as this devices mapper
    /// </summary>
    /// <param name="mapper">the <see cref="IModbusMapper"/> used</param>
    void Register(IModbusMapper mapper);
}
