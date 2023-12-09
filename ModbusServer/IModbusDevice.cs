using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServer;
internal interface IModbusDevice
{
    void Register(IModbusMapper mapper);
}
