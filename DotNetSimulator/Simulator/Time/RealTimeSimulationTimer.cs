//Author: FCORDT
using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Time;
internal class RealTimeSimulationTimer : ISimulationTimer
{
    private DateTime _startTime;
    private readonly TimeSpan _timeSpan;
    private readonly TimeSpan _realTimeTimeSpan;
    private DateTime _nextStep;

    public RealTimeSimulationTimer(double timeFactor, TimeSpan timeSpan, DateTime startTime)
    {
        _timeSpan = timeSpan;
        _startTime = startTime;
        _realTimeTimeSpan = _timeSpan * timeFactor;
        _nextStep = DateTime.MinValue;
    }

    public bool HasNextStep()
    {
        return true;
    }

    public async Task<TimeStep> GetNextStep()
    {
        if (_nextStep == DateTime.MinValue)
        {
            _nextStep = DateTime.Now;
        }
        _nextStep += _realTimeTimeSpan;
        await Task.Delay(_nextStep - DateTime.Now);
        var rValue = new TimeStep(_startTime, _timeSpan);
        _startTime += _timeSpan;
        return rValue;
    }
}
