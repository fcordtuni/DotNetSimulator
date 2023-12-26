﻿//Author: FCORDT

namespace ModbusServer.Binding;
public interface IModbusMapper
{
    void RegisterCoils(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors);
    Span<bool> GetCoils(IModbusDevice caller);

    void RegisterDiscreteInputs(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors);

    ReadOnlySpan<bool> GetDiscreteInputs(IModbusDevice caller);

    void RegisterHoldingRegisters(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptor);

    Span<short> GetHoldingRegisters(IModbusDevice caller);

    void RegisterInputRegisters(IModbusDevice caller, ICollection<ModbusInterfaceDescriptor> descriptors);

    ReadOnlySpan<short> GetInputRegisters(IModbusDevice caller);
}
