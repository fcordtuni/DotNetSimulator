using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServer;
internal class EasyModbusModbusMapper : IModbusMapper
{
    private EasyModbus.ModbusServer.Coils coils;
    private EasyModbus.ModbusServer.DiscreteInputs discrtetInputs;
    private EasyModbus.ModbusServer.InputRegisters inputRegisters;
    private EasyModbus.ModbusServer.HoldingRegisters holdingRegisters;

    private IDictionary<IModbusDevice, ModbusBinding> coilsBinding;
    private IDictionary<IModbusDevice, ModbusBinding> discreteInputBindung;
    private IDictionary<IModbusDevice, ModbusBinding> inputRegisterBinding;
    private IDictionary<IModbusDevice, ModbusBinding> holdingRegisterBinding;


    public EasyModbusModbusMapper(EasyModbus.ModbusServer server)
    {
        coils = server.coils;
        discrtetInputs = server.discreteInputs;
        inputRegisters = server.inputRegisters;
        holdingRegisters = server.holdingRegisters;
    }

    public void RegisterCoils(IModbusDevice caller, ModbusBinding descriptors)
    {
        coilsBinding[caller] = descriptors;
    }

    public Span<bool> GetCoils(IModbusDevice caller)
    {
        return coilsBinding.TryGetValue(caller, out var binding)
            ? ((Span<bool>)coils.localArray).Slice(binding.Start, binding.End - binding.Start)
            : default;
    }

    public void RegisterDiscreteInputs(IModbusDevice caller, ModbusBinding descriptors)
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<bool> GetDiscreteInputs(IModbusDevice caller)
    {
        throw new NotImplementedException();
    }

    public void RegisterHoldingRegisters(IModbusDevice caller, ModbusBinding descriptor)
    {
        throw new NotImplementedException();
    }

    public Span<short> GetHoldingRegisters(IModbusDevice caller)
    {
        throw new NotImplementedException();
    }

    public void RegisterInputRegisters(IModbusDevice caller, ModbusBinding descriptors)
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<short> GetInputRegisters(IModbusDevice caller)
    {
        throw new NotImplementedException();
    }
}
