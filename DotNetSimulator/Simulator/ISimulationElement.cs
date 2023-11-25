//Author: FCORDT
using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator;

/// <summary>
/// The basic Interface for Simulation Elements
/// </summary>
public interface ISimulationElement
{
    /// <summary>
    /// Simulate a step, possibly consuming power from the <paramref name="producers"/> 
    /// </summary>
    /// <param name="step"></param>
    /// <param name="producers"></param>
    void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> producers);

    /// <summary>
    /// Consume up to <paramref name="maxAmount"/> KWH of Power
    /// </summary>
    /// <param name="maxAmount"></param>
    /// <returns></returns>
    KWH GetProduction(KWH maxAmount);

    public KWH GetProduction() => GetProduction(KWH.Infinity);
}
