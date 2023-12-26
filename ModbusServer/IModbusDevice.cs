//Author: FCORDT

using ModbusServer.Binding;

namespace ModbusServer;
public interface IModbusDevice
{
    void Register(IModbusMapper mapper);
}
