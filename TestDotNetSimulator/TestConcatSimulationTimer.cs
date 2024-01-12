//Author: Elisabeth Gisser

using DotNetSimulator.Simulator.Time;
using DotNetSimulator.Units;

namespace TestDotNetSimulator;

public class ConcatSimulationTimerTests
{
    public class TestSimulationTimer : ISimulationTimer
    {
        private bool hasNextStep;
        private TimeStep timeStep;

        public TestSimulationTimer(bool hasNextStep, TimeStep timeStep)
        {
            this.hasNextStep = hasNextStep;
            this.timeStep = timeStep;
        }

        public bool HasNextStep()
        {
            return hasNextStep;
        }

        public Task<TimeStep> GetNextStep()
        {
            return Task.FromResult(timeStep);
        }
    }

    [Fact]
    public async Task HasNextStep_WhenEitherTimerHasNextStep_ReturnsTrue()
    {
        var firstTimer = new TestSimulationTimer(true, new TimeStep());
        var secondTimer = new TestSimulationTimer(false, new TimeStep());

        var concatTimer = new ConcatSimulationTimer(firstTimer, secondTimer);

        var hasNextStep = concatTimer.HasNextStep();

        Assert.True(hasNextStep);
    }

    [Fact]
    public async Task HasNextStep_WhenBothTimersNoNextStep_ReturnsFalse()
    {
        var firstTimer = new TestSimulationTimer(false, new TimeStep());
        var secondTimer = new TestSimulationTimer(false, new TimeStep());

        var concatTimer = new ConcatSimulationTimer(firstTimer, secondTimer);

        var hasNextStep = concatTimer.HasNextStep();

        Assert.False(hasNextStep);
    }

    [Fact]
    public async Task GetNextStep_WhenFirstTimerHasNextStep_ReturnsFirstTimerNextStep()
    {
        var expectedTimeStep = new TimeStep();
        var firstTimer = new TestSimulationTimer(true, expectedTimeStep);
        var secondTimer = new TestSimulationTimer(false, new TimeStep());

        var concatTimer = new ConcatSimulationTimer(firstTimer, secondTimer);

        var nextStep = await concatTimer.GetNextStep();

        Assert.Equal(expectedTimeStep, nextStep);
    }

    [Fact]
    public async Task GetNextStep_WhenFirstTimerHasNoNextStep_ReturnsSecondTimerNextStep()
    {
        var expectedTimeStep = new TimeStep();
        var firstTimer = new TestSimulationTimer(false, new TimeStep());
        var secondTimer = new TestSimulationTimer(true, expectedTimeStep);

        var concatTimer = new ConcatSimulationTimer(firstTimer, secondTimer);

        var nextStep = await concatTimer.GetNextStep();

        Assert.Equal(expectedTimeStep, nextStep);
    }
}