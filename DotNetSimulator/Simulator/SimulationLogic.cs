using DotNetSimulator.Units;
using DotNetSimulator.Utils;
using Serilog;

namespace DotNetSimulator.Simulator
{
    internal class SimulationLogic
    {
        private readonly DAG<ISimulationElement> _powerGrid;
        private IList<ISimulationElement> _simulationOrder;
        public SimulationLogic() 
        { 
            _powerGrid = new DAG<ISimulationElement>();
            _simulationOrder = new List<ISimulationElement>();
        }

        private void OrderGrid()
        {
            _simulationOrder = _powerGrid.TopologicalSort();
        }

        public void AddLink(ISimulationElement from, ISimulationElement to)
        {
            _powerGrid.AddEdge(from, to);
            Log.Information("Adding link {from} - {to}", from, to);
            OrderGrid();
        }

        public void AddLinks((ISimulationElement, ISimulationElement)[] links)
        {
            foreach (var link in links)
            {
                _powerGrid.AddEdge(link.Item1, link.Item2);
                Log.Information("Adding link {from} - {to}", link.Item1, link.Item2);
            }

            OrderGrid();
        }

        private static void WaitForTime(DateTime time)
        {
            while (DateTime.Now < time)
            {
                Thread.Sleep(1);
            }
        }

        public void RealTimeSimulation(DateTime start, TimeSpan stepSize)
        {
            var simulationStep = new TimeStep(start, stepSize);
            while (true)
            {
                WaitForTime(simulationStep.End);
                Log.Information("Simulating Step {step}", simulationStep);
                SimulateStep(simulationStep, _simulationOrder);
                simulationStep = simulationStep.Next(stepSize);
            }
        }

        private void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> orderedNodes)
        {
            foreach(var currentNode in orderedNodes)
            {
                currentNode.SimulateStep(step, _powerGrid.IncomingNodes(currentNode));
            }
        }
    }
}
