using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServer;
internal struct ModbusBinding
{
    public ModbusBinding(ICollection<ModbusInterfaceDescriptor> interfaces)
    {
        Interfaces = interfaces;
        Start = interfaces.Select(i => i.Offset).Min();
        End = interfaces.Select(i => i.Offset + i.Length).Max();
    }

    public int Start { get; }
    public int End { get; }

    public ICollection<ModbusInterfaceDescriptor> Interfaces { get; }

}
