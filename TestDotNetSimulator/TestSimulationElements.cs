using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DotNetSimulator.Simulator;
using DotNetSimulator.Units;
using NLog.LayoutRenderers;

namespace TestDotNetSimulator
{
    internal interface ISimulationTestElement : ISimulationElement
    {
        KWH TotalOutgoing();
        KWH TotalIncoming();
    }
    internal class TimeInvariantSimulationElement(KWH maxProduction, KWH maxConsumption) : ISimulationTestElement
    {
        private readonly KWH _maxProduction = maxProduction;
        private readonly KWH _maxConsumption = maxConsumption;
        private KWH _produced = KWH.Zero;
        private KWH _consumed = KWH.Zero;

        public void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> producers)
        {
            foreach (var producer in producers)
            {
                var remaining = _maxConsumption - _consumed;
                if (remaining.Amount > 0)
                {
                    _consumed += producer.GetProduction(remaining);
                }
            }
        }

        public KWH GetProduction(KWH maxAmount)
        {
            var producing = KWH.Min(_maxProduction - _produced, maxAmount);
            _produced += producing;
            return producing;
        }

        public KWH TotalOutgoing()
        {
            return _produced;
        }

        public KWH TotalIncoming()
        {
            return _consumed;
        }
    }
    internal static class TestSimulationElements
    {
        internal static ISimulationTestElement InfiniteProducer() =>
            new TimeInvariantSimulationElement(KWH.Infinity, KWH.Zero);

        internal static ISimulationTestElement InfiniteConsumer() =>
            new TimeInvariantSimulationElement(KWH.Zero, KWH.Infinity);
    }
}
