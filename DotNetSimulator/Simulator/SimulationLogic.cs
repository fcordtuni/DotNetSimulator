//Author: FCORDT

using DotNetSimulator.Simulator.Time;
using DotNetSimulator.Units;
using DotNetSimulator.Utils;
using NLog;
using ILogger = NLog.ILogger;

namespace DotNetSimulator.Simulator
{
    internal class SimulationLogic
    {
        private readonly DAG<ISimulationElement> _powerGrid = new();
        private IList<ISimulationElement> _simulationOrder = new List<ISimulationElement>();
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private ISimulationTimer _simulationTimer = new RealTimeSimulationTimer(1, TimeSpan.FromSeconds(1), DateTime.Now);
        private bool _shouldRun = true;
        private TimeStep _currentStep;

        public SimulationLogic()
        {

        }

        public SimulationLogic(ISimulationTimer timer)
        {
            _simulationTimer = timer;
        }

        public void SetFastForward(DateTime endTime, TimeSpan simulationResolution, double realTimeSimulationSpeed)
        {
            _simulationTimer =
                ISimulationTimer.FastForward(_currentStep.End, endTime, simulationResolution)
                    .AndThen(ISimulationTimer.RealTime(realTimeSimulationSpeed, simulationResolution, endTime));
        }

        public void SetRealTime(double realTimeSimulationSpeed, TimeSpan simulationResolution)
        {
            _simulationTimer = ISimulationTimer.RealTime(realTimeSimulationSpeed, simulationResolution, _currentStep.End);
        }

        public void Stop()
        {
            _shouldRun = false;
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

        public void AddLinks(IEnumerable<(ISimulationElement, ISimulationElement)> links)
        {
            foreach (var link in links)
            {
                _powerGrid.AddEdge(link.Item1, link.Item2);
                Logger.Info("Adding link {from} - {to}", link.Item1, link.Item2);
            }

            OrderGrid();
        }

        public async Task<bool> RunSimulation()
        {
            while (_shouldRun)
            {
                _currentStep = await _simulationTimer.GetNextStep();
                Logger.Info("Simulating Step {step}", _currentStep);
                SimulateStep(_currentStep, _simulationOrder);
            }
            return true;
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
