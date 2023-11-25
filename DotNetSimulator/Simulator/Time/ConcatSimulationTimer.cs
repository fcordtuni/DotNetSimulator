//Author: FCORDT
using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Time;

internal class ConcatSimulationTimer : ISimulationTimer
{
    private readonly ISimulationTimer _first;
    private readonly ISimulationTimer _second;

    internal ConcatSimulationTimer(ISimulationTimer first, ISimulationTimer second)
    {
        _first = first;
        _second = second;
    }

    /// <inheritdoc />
    public bool HasNextStep()
    {
        return _first.HasNextStep() || _second.HasNextStep();
    }

    /// <inheritdoc />
    public async Task<TimeStep> GetNextStep()
    {
        if (_first.HasNextStep())
        {
            return await _first.GetNextStep();
        }
        return await _second.GetNextStep();
    }
}
