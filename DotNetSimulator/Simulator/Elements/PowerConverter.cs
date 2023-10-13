using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Elements
{
    internal class PowerConverter : ISimulationElement
    {
        private readonly string _name;
        private KWH _stepProduction;

        public PowerConverter(string name)
        {
            this._stepProduction = KWH.Zero;
            this._name = name;
        }

        public string Name => "Power Converter " + _name;

        public KWH GetProduction(KWH maxAmount)
        {
            var providablePower = KWH.Min(maxAmount, _stepProduction);
            _stepProduction -= providablePower;
            return providablePower;
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            _stepProduction = producers.Select(x => x.GetProduction()).Aggregate((x, y) => x + y);
        }
    }
}
