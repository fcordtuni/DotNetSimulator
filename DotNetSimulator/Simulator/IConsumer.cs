using DotNetSimulator.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Simulator
{
    internal interface IConsumer
    {
        void Consume(KWH amount);
    }
}
