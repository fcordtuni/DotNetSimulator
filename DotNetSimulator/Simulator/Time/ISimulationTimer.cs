﻿//Author: FCORDT
using DotNetSimulator.Units;

namespace DotNetSimulator.Simulator.Time
{
    internal interface ISimulationTimer
    {
        bool HasNextStep();
        Task<TimeStep> GetNextStep();

        ISimulationTimer AndThen(ISimulationTimer next)
        {
            return new ConcatSimulationTimer(this, next);
        }

        static ISimulationTimer FastForward(DateTime startTime, DateTime endTime, TimeSpan timeSpan)
        {
            return new FastForwardSimulationTimer(startTime, endTime, timeSpan);
        }

        static ISimulationTimer RealTime(double timeFactor, TimeSpan timeSpan, DateTime startTime)
        {
            return new RealTimeSimulationTimer(timeFactor, timeSpan, startTime);
        }
    }
}