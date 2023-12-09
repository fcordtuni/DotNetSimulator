using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServer.Binding;
internal class EasyModbusModbusMapper : IModbusMapper
{
    private EasyModbus.ModbusServer.Coils coils;
    private EasyModbus.ModbusServer.DiscreteInputs discrtetInputs;
    private EasyModbus.ModbusServer.InputRegisters inputRegisters;
    private EasyModbus.ModbusServer.HoldingRegisters holdingRegisters;

    private IModbusBindingManager<bool> coilsBindingManager;
    private IModbusBindingManager<bool> discreteInputBindingManager;
    private IModbusBindingManager<short> inputRegisterBindingManager;
    private IModbusBindingManager<short> holdingRegisterBindingManager;


    public EasyModbusModbusMapper(EasyModbus.ModbusServer server)
    {
        coils = server.coils;
        discrtetInputs = server.discreteInputs;
        inputRegisters = server.inputRegisters;
        holdingRegisters = server.holdingRegisters;
    }

    public void RegisterCoils(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors)
    {
        coilsBindingManager.RegisterBinding(caller, descriptors);
    }

    public Span<bool> GetCoils(IModbusDevice caller)
    {
        return coilsBindingManager.GetBinding(caller);
    }

    public void RegisterDiscreteInputs(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors)
    {
        discreteInputBindingManager.RegisterBinding(caller, descriptors);
    }

    public ReadOnlySpan<bool> GetDiscreteInputs(IModbusDevice caller)
    {
        return discreteInputBindingManager.GetBinding(caller);
    }

    public void RegisterHoldingRegisters(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptor)
    {
        holdingRegisterBindingManager.RegisterBinding(caller, descriptor);
    }

    public Span<short> GetHoldingRegisters(IModbusDevice caller)
    {
        return holdingRegisterBindingManager.GetBinding(caller);
    }

    public void RegisterInputRegisters(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors)
    {
        inputRegisterBindingManager.RegisterBinding(caller,descriptors);
    }

    public ReadOnlySpan<short> GetInputRegisters(IModbusDevice caller)
    {
        return inputRegisterBindingManager.GetBinding(caller);
    }
}
