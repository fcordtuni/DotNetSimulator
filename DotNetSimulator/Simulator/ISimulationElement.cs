using DotNetSimulator.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Simulator
{
    internal interface ISimulationElement
    {
        void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers);
        KWH GetProduction(KWH maxAmount);

        public KWH GetProduction() => GetProduction(KWH.Infinity);

        string Name { get; }
    }
}
