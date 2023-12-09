using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServer;
internal interface IModbusMapper
{
    void RegisterCoils(IModbusDevice caller, ModbusBinding descriptors);
    Span<bool> GetCoils(IModbusDevice caller);

    void RegisterDiscreteInputs(IModbusDevice caller, ModbusBinding descriptors);

    ReadOnlySpan<bool> GetDiscreteInputs(IModbusDevice caller);

    void RegisterHoldingRegisters(IModbusDevice caller, ModbusBinding descriptor);

    Span<short> GetHoldingRegisters(IModbusDevice caller);

    void RegisterInputRegisters(IModbusDevice caller, ModbusBinding descriptors);

    ReadOnlySpan<short> GetInputRegisters(IModbusDevice caller);
}
