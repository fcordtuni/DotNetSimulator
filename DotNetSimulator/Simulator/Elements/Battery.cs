using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Elements
{
    internal class Battery : ISimulationElement
    {
        private readonly KWH _capacity;
        private readonly KW _maximumInput;
        private readonly KW _maximumOutput;
        private KWH _currentStepMaximumOutput;
        private KWH _currentStepOutput;
        private KWH _currentChargeLevel;
        private readonly string _name;

        public Battery(KWH capacity, string name, KW maximumOutput, KW maximumInput)
        {
            _name = name;
            _maximumOutput = maximumOutput;
            _maximumInput = maximumInput;
            _capacity = capacity;
            _currentChargeLevel = KWH.Zero;
            _currentStepMaximumOutput = KWH.Zero;
            _currentStepOutput = KWH.Zero;
        }

        public string Name => "Battery " + _name;

        private KWH CalculateMaximumOutput(KWH maxAmount)
        {
            var remainingProvidablePower = _currentStepMaximumOutput - _currentStepOutput;
            var providablePower = KWH.Min(remainingProvidablePower, _currentChargeLevel);
            return KWH.Min(providablePower, maxAmount);
        }

        public KWH GetProduction(KWH maxAmount)
        {
            var providablePower = CalculateMaximumOutput(maxAmount);
            _currentChargeLevel -= providablePower;
            _currentStepOutput += providablePower;
            return providablePower;
        }

        private KWH CalculateMaximumInput(TimeStep step)
        {
            var maximumStepInput = _maximumInput * step.Duration;
            var remainingCapacity = _capacity - _currentChargeLevel;
            return KWH.Min(remainingCapacity, maximumStepInput);
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            var maximumInput = CalculateMaximumInput(step);
            var currentInput = producers.Aggregate(KWH.Zero, (current, producer) => current + producer.GetProduction(maximumInput - current));
            _currentChargeLevel += currentInput;
            _currentStepMaximumOutput = _maximumOutput * step.Duration;
        }
    }
}
