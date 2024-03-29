﻿//Author: FCORDT
namespace DotNetSimulator.Units;

/// <summary>
/// represents a step in time, consisting of a start, and an end
/// </summary>
public readonly struct TimeStep
{
    /// <summary>
    /// the start of the step
    /// </summary>
    public DateTime Start { get; }

    /// <summary>
    /// the end of the step
    /// </summary>
    public DateTime End { get; }

    /// <summary>
    /// the duration, being end - start
    /// </summary>
    public TimeSpan Duration { get; }

    public static TimeStep Clamp(TimeStep duration, TimeSpan min, TimeSpan max)
    {
        var minTime = duration.Start.TimeOfDay;
        if (min > minTime)
        {
            minTime = min;
        }
        var maxTime = duration.End.TimeOfDay;
        if (max < maxTime)
        {
            maxTime = max;
        }

        return new TimeStep(duration.Start.Date + minTime, duration.End.Date + maxTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public TimeStep(DateTime start, DateTime end) { Start = start; End = end; Duration = end - start; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="duration"></param>
    public TimeStep(DateTime start, TimeSpan duration) { Start = start; End = start + duration; Duration = duration; }

    /// <summary>
    /// returns the next <see cref="TimeStep"/> after this having <paramref name="duration"/>
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public TimeStep Next(TimeSpan duration) => new(End, duration);

    /// <inheritdoc/>
    public override string ToString()
    {
        return Start + " - " + End;
    }
}
