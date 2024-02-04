//Author: FCORDT
using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Time;
internal class RealTimeSimulationTimer : ISimulationTimer
{
    private DateTime _startTime;
    private readonly TimeSpan _timeSpan;
    private readonly TimeSpan _realTimeTimeSpan;
    private DateTime _nextStep;

    internal RealTimeSimulationTimer(double timeFactor, TimeSpan timeSpan, DateTime startTime)
    {
        _timeSpan = timeSpan;
        _startTime = startTime;
        _realTimeTimeSpan = _timeSpan * timeFactor;
        _nextStep = DateTime.MinValue;
    }

    /// <inheritdoc />
    public bool HasNextStep()
    {
        return true;
    }

    /// <inheritdoc />
    public async Task<TimeStep> GetNextStep()
    {
        if (_nextStep == DateTime.MinValue)
        {
            _nextStep = DateTime.Now;
        }
        _nextStep += _realTimeTimeSpan;
        var delay = _nextStep - DateTime.Now;
        if (delay.Ticks > 0)
        {
            await Task.Delay(delay);
        }

        var rValue = new TimeStep(_startTime, _timeSpan);
        _startTime += _timeSpan;
        return rValue;
    }
}
