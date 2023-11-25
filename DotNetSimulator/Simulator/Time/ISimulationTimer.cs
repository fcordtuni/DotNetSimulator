//Author: FCORDT
using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Time;

/// <summary>
/// Describes a async Simulation Timer Type, using async awaits to control time flow in simulation as well as real time
/// </summary>
public interface ISimulationTimer
{
    /// <summary>
    /// Does there exist a next simulation step after the current one?
    /// </summary>
    /// <returns></returns>
    bool HasNextStep();
    public Task<TimeStep> GetNextStep();

    /// <summary>
    /// Concatenates this <see cref="ISimulationTimer"/> with another one.
    /// </summary>
    /// <param name="next"></param>
    /// <returns>A <see cref="ISimulationTimer"/> that simulates this timer first, and then the <paramref name="next"/></returns>
    ISimulationTimer AndThen(ISimulationTimer next)
    {
        return new ConcatSimulationTimer(this, next);
    }

    /// <summary>
    /// Creates a <see cref="ISimulationTimer"/> that fast-forwards as fast as possible
    /// starting at <paramref name="startTime"/>, and ending at <paramref name="endTime"/>
    /// at <paramref name="timeSpan"/> simulation steps
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    static ISimulationTimer FastForward(DateTime startTime, DateTime endTime, TimeSpan timeSpan)
    {
        return new FastForwardSimulationTimer(startTime, endTime, timeSpan);
    }


    /// <summary>
    /// Creates a <see cref="ISimulationTimer"/> that runs in real time with time-factor <paramref name="timeFactor"/>
    /// starting at <paramref name="startTime"/>, endlessly
    /// at <paramref name="timeSpan"/> simulation steps
    /// </summary>
    /// <param name="timeFactor"></param>
    /// <param name="timeSpan"></param>
    /// <param name="startTime"></param>
    /// <returns></returns>
    static ISimulationTimer RealTime(double timeFactor, TimeSpan timeSpan, DateTime startTime)
    {
        return new RealTimeSimulationTimer(timeFactor, timeSpan, startTime);
    }
}
