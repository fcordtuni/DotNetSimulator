using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Elements
{
    internal class LoggingDecorator : ISimulationElement
    {
        private readonly ISimulationElement _decoratee;

        public LoggingDecorator(ISimulationElement decoratee)
        {
            _decoratee = decoratee;
        }

        public string Name => _decoratee.Name;

        public KWH GetProduction(KWH maxAmount)
        {
            var produced = _decoratee.GetProduction(maxAmount);
            Console.WriteLine(_decoratee.Name + ": produced " +  produced + "!");
            return produced;
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            _decoratee.SimulateStep(step, producers);
        }
    }
}
