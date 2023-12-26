//Author: FCORDT

namespace ModbusServer.Binding;
public readonly struct ModbusInterfaceDescriptor(int offset, int length, string description)
{
    public int Offset { get; } = offset;
    public int Length { get; } = length;
    public string Description { get; } = description;

}
