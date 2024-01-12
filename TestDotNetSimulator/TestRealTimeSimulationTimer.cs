﻿//Author: Elisabeth Gisser

using DotNetSimulator.Time;
using System.Threading.Tasks;

namespace TestDotNetSimulator;
public class TestRealTimeSimulationTimer
{
    public class TestSimulationTimer : ISimulationTimer
    {
        private DateTime currentTime;
        private TimeSpan timeSpan;

        public TestSimulationTimer(DateTime startTime, TimeSpan timeSpan)
        {
            currentTime = startTime;
            this.timeSpan = timeSpan;
        }

        public bool HasNextStep()
        {
            return true;
        }

        public Task<TimeStep> GetNextStep()
        {
            var rValue = new TimeStep(currentTime, timeSpan);
            currentTime += timeSpan;
            return Task.FromResult(rValue);
        }
    }

    [Fact]
    public async Task GetNextStep_ReturnsExpectedTimeStep()
    {
        DateTime startTime = DateTime.Now;
        TimeSpan timeSpan = TimeSpan.FromSeconds(5);
        double timeFactor = 2.0;

        var testTimer = new TestSimulationTimer(startTime, timeSpan);
        var realTimeTimer = new RealTimeSimulationTimer(timeFactor, timeSpan, startTime);

        for (int i = 0; i < 5; i++)
        {
            var testNextStep = await testTimer.GetNextStep();
            var realTimeNextStep = await realTimeTimer.GetNextStep();

            var timeDifference = Math.Abs((realTimeNextStep.Timestamp - testNextStep.Timestamp).TotalSeconds);
            Assert.True(timeDifference <= 1, "Time difference should be within 1 second tolerance.");
        }
    }

    [Fact]
    public async Task GetNextStep_ReturnsStepsWithIncreasedTimestamp()
    {
        DateTime startTime = DateTime.Now;
        TimeSpan timeSpan = TimeSpan.FromSeconds(3);
        double timeFactor = 1.0;

        var realTimeTimer = new RealTimeSimulationTimer(timeFactor, timeSpan, startTime);

        DateTime previousTimestamp = DateTime.MinValue;

        for (int i = 0; i < 5; i++)
        {
            var realTimeNextStep = await realTimeTimer.GetNextStep();

            Assert.True(realTimeNextStep.Timestamp > previousTimestamp,
                "Timestamp of next step should be greater than the previous step.");
            previousTimestamp = realTimeNextStep.Timestamp;
        }
    }

    [Fact]
    public async Task GetNextStep_ReturnsStepsInRealTime()
    {
        DateTime startTime = DateTime.Now;
        TimeSpan timeSpan = TimeSpan.FromSeconds(3);
        double timeFactor = 2.0;

        var realTimeTimer = new RealTimeSimulationTimer(timeFactor, timeSpan, startTime);

        DateTime previousTimestamp = DateTime.MinValue;

        for (int i = 0; i < 5; i++)
        {
            var realTimeNextStep = await realTimeTimer.GetNextStep();

            var timeDifference = (DateTime.Now - realTimeNextStep.Timestamp).TotalSeconds;
            Assert.True(timeDifference < 0.5,
                "Time difference between current time and timestamp should be less than 0.5 seconds.");
        }
    }
}
