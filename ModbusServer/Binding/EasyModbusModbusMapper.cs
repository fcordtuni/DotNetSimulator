﻿//Author: FCORDT

namespace ModbusServer.Binding;
internal class EasyModbusModbusMapper(EasyModbus.ModbusServer server) : IModbusMapper
{
    private readonly IModbusBindingManager<bool> _coilsBindingManager = new EasyModbusModbusBindingManager<bool>(server.coils.localArray);
    private readonly IModbusBindingManager<bool> _discreteInputBindingManager = new EasyModbusModbusBindingManager<bool>(server.discreteInputs.localArray);
    private readonly IModbusBindingManager<short> _inputRegisterBindingManager = new EasyModbusModbusBindingManager<short>(server.inputRegisters.localArray);
    private readonly IModbusBindingManager<short> _holdingRegisterBindingManager = new EasyModbusModbusBindingManager<short>(server.holdingRegisters.localArray);


    public void RegisterCoils(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors)
    {
        _coilsBindingManager.RegisterBinding(caller, descriptors);
    }

    public Span<bool> GetCoils(IModbusDevice caller)
    {
        return _coilsBindingManager.GetBinding(caller);
    }

    public void RegisterDiscreteInputs(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors)
    {
        _discreteInputBindingManager.RegisterBinding(caller, descriptors);
    }

    public ReadOnlySpan<bool> GetDiscreteInputs(IModbusDevice caller)
    {
        return _discreteInputBindingManager.GetBinding(caller);
    }

    public void RegisterHoldingRegisters(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptor)
    {
        _holdingRegisterBindingManager.RegisterBinding(caller, descriptor);
    }

    public Span<short> GetHoldingRegisters(IModbusDevice caller)
    {
        return _holdingRegisterBindingManager.GetBinding(caller);
    }

    public void RegisterInputRegisters(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors)
    {
        _inputRegisterBindingManager.RegisterBinding(caller,descriptors);
    }

    public ReadOnlySpan<short> GetInputRegisters(IModbusDevice caller)
    {
        return _inputRegisterBindingManager.GetBinding(caller);
    }
}
