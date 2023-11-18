//Author: FCORDT
using DotNetSimulator.Units;
using DotNetSimulator.Utils;
using NLog;
using ILogger = NLog.ILogger;

namespace DotNetSimulator.Simulator
{
    internal class SimulationLogic
    {
        private readonly DAG<ISimulationElement> _powerGrid;
        private IList<ISimulationElement> _simulationOrder;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
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
            Logger.Info("Adding link {from} - {to}", from, to);
            OrderGrid();
        }

        public void AddLinks((ISimulationElement, ISimulationElement)[] links)
        {
            foreach (var link in links)
            {
                _powerGrid.AddEdge(link.Item1, link.Item2);
                Logger.Info("Adding link {from} - {to}", link.Item1, link.Item2);
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
                Logger.Info("Simulating Step {step}", simulationStep);
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
