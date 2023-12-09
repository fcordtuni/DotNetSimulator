using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusServer.Binding;
internal readonly struct ModbusInterfaceDescriptor
{
    public ModbusInterfaceDescriptor(int offset, int length, string description)
    {
        Offset = offset;
        Length = length;
        Description = description;
    }

    public int Offset { get; }
    public int Length { get; }
    public string Description { get; }

}
