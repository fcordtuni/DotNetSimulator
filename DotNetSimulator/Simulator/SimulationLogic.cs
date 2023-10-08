using DotNetSimulator.Units;
using DotNetSimulator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Simulator
{
    internal class SimulationLogic
    {
        private DAG<ISimulationElement> powerGrid;
        public SimulationLogic() 
        { 
            powerGrid = new DAG<ISimulationElement>();
        }

        public void Simulate(DateTime from, DateTime to, TimeSpan stepSize)
        {
            List<ISimulationElement> orderedNodes = powerGrid.TopologicalSort();
            TimeStep step = new TimeStep(from, from + stepSize);
            while(step.Start < to)
            {
                SimulateStep(step, orderedNodes);
                step = step.Next(stepSize);
            }

        }

        private void SimulateStep(TimeStep step, List<ISimulationElement> orderedNodes)
        {
            foreach(ISimulationElement currentNode in orderedNodes)
            {
                currentNode.SimulateStep(step, powerGrid.IncomingNodes(currentNode));
            }
        }
    }
}
