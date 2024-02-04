//Author: Elisabeth Gisser

using DotNetSimulator.Simulator.Time;
using DotNetSimulator.Units;

namespace TestDotNetSimulator;
public class TestRealTimeSimulationTimer
{
    private class TestSimulationTimer(DateTime startTime, TimeSpan timeSpan) : ISimulationTimer
    {
        public bool HasNextStep()
        {
            return true;
        }

        public Task<TimeStep> GetNextStep()
        {
            var rValue = new TimeStep(startTime, timeSpan);
            startTime += timeSpan;
            return Task.FromResult(rValue);
        }
    }

    [Fact]
    public async Task GetNextStep_ReturnsExpectedTimeStep()
    {
        var startTime = DateTime.Now;
        var timeSpan = TimeSpan.FromSeconds(5);
        const double timeFactor = 2.0;

        var testTimer = new TestSimulationTimer(startTime, timeSpan);
        var realTimeTimer = new RealTimeSimulationTimer(timeFactor, timeSpan, startTime);

        for (var i = 0; i < 5; i++)
        {
            var testNextStep = await testTimer.GetNextStep();
            var realTimeNextStep = await realTimeTimer.GetNextStep();

            var timeDifference = Math.Abs((realTimeNextStep.Start - testNextStep.Start).TotalSeconds);
            Assert.True(timeDifference <= 1, "Time difference should be within 1 second tolerance.");
        }
    }

    [Fact]
    public async Task GetNextStep_ReturnsStepsWithIncreasedTimestamp()
    {
        var startTime = DateTime.Now;
        var timeSpan = TimeSpan.FromSeconds(3);
        const double timeFactor = 1.0;

        var realTimeTimer = new RealTimeSimulationTimer(timeFactor, timeSpan, startTime);

        var previousTimestamp = DateTime.MinValue;

        for (var i = 0; i < 5; i++)
        {
            var realTimeNextStep = await realTimeTimer.GetNextStep();

            Assert.True(realTimeNextStep.Start > previousTimestamp,
                "Timestamp of next step should be greater than the previous step.");
            previousTimestamp = realTimeNextStep.Start;
        }
    }

    [Fact]
    public async Task GetNextStep_ReturnsStepsInRealTime()
    {
        var startTime = DateTime.Now;
        var timeSpan = TimeSpan.FromSeconds(3);
        const double timeFactor = 2.0;

        var realTimeTimer = new RealTimeSimulationTimer(timeFactor, timeSpan, startTime);

        for (var i = 0; i < 5; i++)
        {
            var realTimeNextStep = await realTimeTimer.GetNextStep();

            var timeDifference = (DateTime.Now - realTimeNextStep.Start).TotalSeconds;
            Assert.True(timeDifference < 0.5,
                "Time difference between current time and timestamp should be less than 0.5 seconds.");
        }
    }
}
