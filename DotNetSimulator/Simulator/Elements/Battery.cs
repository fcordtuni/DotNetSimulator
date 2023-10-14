using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Elements
{
    internal class Battery : ISimulationElement
    {
        private readonly KWH _capacity;
        private KWH _currentLevel;
        private readonly string _name;

        public Battery(KWH capacity, string name)
        {
            _name = name;
            _capacity = capacity;
            _currentLevel = KWH.Zero;
        }

        public string Name => "Battery " + _name;

        public KWH GetProduction(KWH maxAmount)
        {
            var providablePower = KWH.Min(maxAmount, _currentLevel);
            _currentLevel -= providablePower;
            return providablePower;
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            //for now: accept all power!
            foreach(var producer in producers)
            {
                _currentLevel += producer.GetProduction(_capacity - _currentLevel);
            }
        }
    }
}
