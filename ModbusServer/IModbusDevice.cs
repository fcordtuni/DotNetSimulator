using ModbusServer.Binding;

namespace ModbusServer;
internal interface IModbusDevice
{
    void Register(IModbusMapper mapper);
}
