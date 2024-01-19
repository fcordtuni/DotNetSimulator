//Author: FCORDT

namespace ModbusDeviceLibrary.Modbus;

/// <summary>
/// Describes a Modbus Interface, i.e. a number of coils or registers or ... to a single value
///
/// </summary>
/// <param name="offset">The local offset (starting a 0) for this value mapping</param>
/// <param name="length">The length this value has</param>
/// <param name="description">A verbal description of the value</param>
public readonly struct ModbusInterfaceDescriptor(int offset, int length, string description)
{
    public int Offset { get; } = offset;
    public int Length { get; } = length;
    public string Description { get; } = description;

}
