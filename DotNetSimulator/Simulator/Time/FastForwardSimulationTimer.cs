//Author: FCORDT
using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Time;
internal class FastForwardSimulationTimer : ISimulationTimer
{
    private readonly DateTime _endTime;
    private DateTime _startTime;
    private readonly TimeSpan _timeSpan;

    public FastForwardSimulationTimer(DateTime startTime, DateTime endTime, TimeSpan timeSpan)
    {
        _startTime = startTime;
        _endTime = endTime;
        _timeSpan = timeSpan;
    }

    public bool HasNextStep()
    {
        return _startTime < _endTime;
    }

    public Task<TimeStep> GetNextStep()
    {
        _startTime += _timeSpan;
        return Task.Run(() => new TimeStep(_startTime, _timeSpan));
    }
}
