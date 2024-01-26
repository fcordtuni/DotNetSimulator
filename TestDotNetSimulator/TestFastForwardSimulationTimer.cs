//Author: Elisabeth Gisser

using DotNetSimulator.Simulator.Time;

namespace TestDotNetSimulator;
public class TestFastForwardSimulationTimer
{
    [Fact]
    public void HasNextStep_WhenEndTimeNotReached_ReturnsTrue()
    {
        DateTime startTime = new DateTime(2023, 1, 1);
        DateTime endTime = new DateTime(2023, 1, 10);
        TimeSpan timeSpan = TimeSpan.FromDays(1);

        var fastForwardTimer = new FastForwardSimulationTimer(startTime, endTime, timeSpan);

        var hasNextStep = fastForwardTimer.HasNextStep();

        Assert.True(hasNextStep);
    }

    [Fact]
    public async Task GetNextStep_ReturnsTimeStepsUntilEndTime()
    {
        DateTime startTime = new DateTime(2023, 1, 1);
        DateTime endTime = new DateTime(2023, 1, 5);
        TimeSpan timeSpan = TimeSpan.FromDays(1);

        var fastForwardTimer = new FastForwardSimulationTimer(startTime, endTime, timeSpan);

        while (fastForwardTimer.HasNextStep())
        {
            var nextStep = await fastForwardTimer.GetNextStep();
            Assert.True(nextStep.Start <= endTime, "Timestamp should be less than end time.");
        }
    }

    [Fact]
    public void HasNextStep_WhenEndTimeReached_ReturnsFalse()
    {
        DateTime startTime = new DateTime(2023, 1, 1);
        DateTime endTime = new DateTime(2023, 1, 3);
        TimeSpan timeSpan = TimeSpan.FromDays(1);

        var fastForwardTimer = new FastForwardSimulationTimer(startTime, endTime, timeSpan);

        while (fastForwardTimer.HasNextStep())
        {
            fastForwardTimer.GetNextStep();
        }

        var hasNextStep = fastForwardTimer.HasNextStep();

        Assert.False(hasNextStep);
    }
}