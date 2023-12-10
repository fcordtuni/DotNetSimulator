namespace ModbusServer.Binding;
internal interface IModbusBindingManager<T>
{
    void RegisterBinding(IModbusDevice device, ICollection<ModbusInterfaceDescriptor> interfaces);
    Span<T> GetBinding(IModbusDevice device);
}

internal class EasyModbusModbusBindingManager<T>(T[] content) : IModbusBindingManager<T>
{
    private readonly IDictionary<IModbusDevice, ModbusBindingDescriptor> _descDict = new Dictionary<IModbusDevice, ModbusBindingDescriptor>();
    private ModbusBindingDescriptor _lastAdded = new();

    public void RegisterBinding(IModbusDevice device, ICollection<ModbusInterfaceDescriptor> interfaces)
    {
        var offset = _lastAdded.End;
        _lastAdded = new ModbusBindingDescriptor(interfaces, offset);
        _descDict[device] = _lastAdded;
    }

    public Span<T> GetBinding(IModbusDevice device)
    {
        var desc = _descDict[device];
        return ((Span<T>)content)[desc.Start..desc.End];
    }
}
