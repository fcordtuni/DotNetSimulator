using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Elements
{
    internal class PowerConverter : ISimulationElement
    {
        private readonly string name;
        private KWH stepProduction;

        public PowerConverter(string name)
        {
            this.stepProduction = KWH.Zero;
            this.name = name;
        }

        public string Name { get => "Power Converter " + name; }

        public KWH GetProduction(KWH maxAmount)
        {
            KWH providablePower = KWH.Min(maxAmount, stepProduction);
            stepProduction -= providablePower;
            return providablePower;
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            stepProduction = producers.Select(x => x.GetProduction()).Aggregate((x, y) => x + y);
        }
    }
}
