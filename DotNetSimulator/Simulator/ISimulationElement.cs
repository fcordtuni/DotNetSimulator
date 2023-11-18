//Author: FCORDT
using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator
{

    internal interface ISimulationElement
    {
        void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers);
        KWH GetProduction(KWH maxAmount);

        public KWH GetProduction() => GetProduction(KWH.Infinity);
    }
}
