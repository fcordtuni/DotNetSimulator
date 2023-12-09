using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServer.Binding
{
    internal interface IModbusBindingManager<T>
    {
        void RegisterBinding(IModbusDevice device, ICollection<ModbusInterfaceDescriptor> interfaces);
        Span<T> GetBinding(IModbusDevice device);
    }
}
