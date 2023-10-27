using DotNetSimulator.Units;
using Serilog;

namespace DotNetSimulator.Simulator.Elements
{
    internal class PowerConverter : ISimulationElement
    {
        private KWH _stepProduction;
        private string _serial;

        public PowerConverter(string serial)
        {
            _stepProduction = KWH.Zero;
            _serial = serial;
            Log.Information("{this}: Created PowerConverter", this);
        }

        public KWH GetProduction(KWH maxAmount)
        {
            var providablePower = KWH.Min(maxAmount, _stepProduction);
            _stepProduction -= providablePower;
            Log.Debug("{this}: Providing {amount}", this, providablePower);
            return providablePower;
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            _stepProduction = producers.Select(x => x.GetProduction()).Aggregate((x, y) => x + y);
            Log.Debug("{this}: Consuming {amount}", this, _stepProduction);
        }

        public override string ToString()
        {
            return "PowerConverter " + _serial;
        }
    }
}
