//Author: FCORDT

using DotNetSimulator.Simulator.Time;
using DotNetSimulator.Units;
using DotNetSimulator.Utils;
using NLog;
using ILogger = NLog.ILogger;

namespace DotNetSimulator.Simulator;

/// <summary>
/// This class manages a Simulation of electrical Consumers and Producers,
/// using an internal <see cref="DAG{ISimulationElement}"/> to order the consumers and producers
/// </summary>
public class SimulationLogic
{
    private readonly DAG<ISimulationElement> _powerGrid = new();
    private IList<ISimulationElement> _simulationOrder = new List<ISimulationElement>();
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private ISimulationTimer _simulationTimer = new RealTimeSimulationTimer(1, TimeSpan.FromSeconds(1), DateTime.Now);
    private bool _shouldRun = true;
    private TimeStep _currentStep;

    /// <summary>
    /// 
    /// </summary>
    public SimulationLogic()
    {

    }

    /// <summary>
    /// Starts the simulation, with an initial seed timer of <paramref name="timer"/>
    /// </summary>
    /// <param name="timer"></param>
    public SimulationLogic(ISimulationTimer timer)
    {
        _simulationTimer = timer;
    }

    /// <summary>
    /// fast forwards the simulation to to the given <paramref name="endTime"/>, then continues at real time
    /// </summary>
    /// <param name="endTime"></param>
    /// <param name="simulationResolution"></param>
    /// <param name="realTimeSimulationSpeed"></param>
    public void SetFastForward(DateTime endTime, TimeSpan simulationResolution, double realTimeSimulationSpeed)
    {
        _simulationTimer =
            ISimulationTimer.FastForward(_currentStep.End, endTime, simulationResolution)
                .AndThen(ISimulationTimer.RealTime(realTimeSimulationSpeed, simulationResolution, endTime));
    }

    /// <summary>
    /// Sets the Simulation to real time
    /// </summary>
    /// <param name="realTimeSimulationSpeed"></param>
    /// <param name="simulationResolution"></param>
    public void SetRealTime(double realTimeSimulationSpeed, TimeSpan simulationResolution)
    {
        _simulationTimer = ISimulationTimer.RealTime(realTimeSimulationSpeed, simulationResolution, _currentStep.End);
    }

    /// <summary>
    /// stops the simulation
    /// </summary>
    public void Stop()
    {
        _shouldRun = false;
    }

    private void OrderGrid()
    {
        _simulationOrder = _powerGrid.TopologicalSort();
    }

    /// <summary>
    /// adds a link between a <see cref="ISimulationElement"/> pair
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void AddLink(ISimulationElement from, ISimulationElement to)
    {
        _powerGrid.AddEdge(from, to);
        Logger.Info("Adding link {from} - {to}", from, to);
        OrderGrid();
    }

    /// <summary>
    /// adds a link between multiple <see cref="ISimulationElement"/> pairs
    /// </summary>
    /// <param name="links"></param>
    public void AddLinks(IEnumerable<(ISimulationElement, ISimulationElement)> links)
    {
        foreach (var link in links)
        {
            _powerGrid.AddEdge(link.Item1, link.Item2);
            Logger.Info("Adding link {from} - {to}", link.Item1, link.Item2);
        }

        OrderGrid();
    }

    /// <summary>
    /// starts the simulation
    /// </summary>
    /// <returns></returns>
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
        foreach (var currentNode in orderedNodes)
        {
            currentNode.SimulateStep(step, _powerGrid.IncomingNodes(currentNode));
        }
    }
}
