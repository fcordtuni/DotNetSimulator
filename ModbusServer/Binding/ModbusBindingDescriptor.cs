//Author: FCORDT

using ModbusDeviceLibrary.Modbus;

namespace ModbusServer.Binding;

/// <summary>
/// This class represents a list of interfaces defined for a single modbus device
///
/// The interfaces each have a local offset, which is added to the global offset to get their defined address on the modbus
/// </summary>
/// <param name="interfaces">The list of interfaces the device uses with their local offset</param>
/// <param name="offset">the global offset the device uses</param>
internal readonly struct ModbusBindingDescriptor(ICollection<ModbusInterfaceDescriptor> interfaces, int offset)
{
    public int Start { get; } = interfaces.Select(i => i.Offset).DefaultIfEmpty().Min() + offset;
    public int End { get; } = interfaces.Select(i => i.Offset + i.Length).DefaultIfEmpty().Max() + offset;

}
