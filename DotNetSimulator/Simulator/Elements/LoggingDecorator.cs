using DotNetSimulator.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Simulator.Elements
{
    internal class LoggingDecorator : ISimulationElement
    {
        private readonly ISimulationElement decoratee;

        public LoggingDecorator(ISimulationElement decoratee)
        {
            this.decoratee = decoratee;
        }

        public string Name => decoratee.Name;

        public KWH GetProduction(KWH maxAmount)
        {
            KWH produced = decoratee.GetProduction(maxAmount);
            Console.WriteLine(decoratee.Name + ": produced " +  produced + "!");
            return produced;
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            decoratee.SimulateStep(step, producers);
        }
    }
}
