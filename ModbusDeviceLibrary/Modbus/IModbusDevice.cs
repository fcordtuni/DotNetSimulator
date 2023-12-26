//Author: FCORDT

namespace ModbusDeviceLibrary.Modbus;
public interface IModbusDevice
{
    void Register(IModbusMapper mapper);
}
