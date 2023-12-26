//Author: FCORDT

namespace ModbusServer.Binding;
internal readonly struct ModbusBindingDescriptor(ICollection<ModbusInterfaceDescriptor> interfaces, int offset)
{
    public int Start { get; } = interfaces.Select(i => i.Offset).DefaultIfEmpty().Min() + offset;
    public int End { get; } = interfaces.Select(i => i.Offset + i.Length).DefaultIfEmpty().Max() + offset;

    public ICollection<ModbusInterfaceDescriptor> Interfaces { get; } = interfaces;

}
