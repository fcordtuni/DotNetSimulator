//Author: FCORDT

using ModbusDeviceLibrary.Modbus;

namespace ModbusServer.Binding;

/// <summary>
/// abstract interface defining a manager for modbus bindings
/// </summary>
/// <typeparam name="T">the type of values that the binding is defining, e.g. boolean for coils</typeparam>
internal interface IModbusBindingManager<T>
{
    // ReSharper disable once UnusedMemberInSuper.Global
    /// <summary>
    /// Allows a modbus device to register a number of interfaces for its use.
    /// The address space on the modbus will be mapped to that device, and the device can use the GetBindung to get a link to their bus space
    /// </summary>
    /// <param name="device"></param>
    /// <param name="interfaces">A (complete) list of modbus interfaces the device plans to use</param>
    void RegisterBinding(IModbusDevice device, ICollection<ModbusInterfaceDescriptor> interfaces);

    // ReSharper disable once UnusedMemberInSuper.Global
    /// <summary>
    /// returns a link to the address space the ModbusDevice has registered
    /// </summary>
    /// <param name="device">the Modbus Device that registered the bindings</param>
    /// <returns>A link to the modbus the device can use to write to</returns>
    Span<T> GetBinding(IModbusDevice device);
}

internal class EasyModbusModbusBindingManager<T>(T[] content) : IModbusBindingManager<T>
{
    private readonly IDictionary<IModbusDevice, ModbusBindingDescriptor> _descDict = new Dictionary<IModbusDevice, ModbusBindingDescriptor>();
    private ModbusBindingDescriptor _lastAdded = new([],1); //start at 1

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
