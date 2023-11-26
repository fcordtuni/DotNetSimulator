//Author: FCORDT
using DotNetSimulator.Units;

namespace TestDotNetSimulator;
public class TestTimeStep
{
    public static IEnumerable<object[]> TimeStepData() =>
        new List<object[]>
        {
            new object[] {new DateTime(2004, 1, 12, 12, 0, 0), new DateTime(2004, 1, 12, 12, 0, 20), TimeSpan.FromSeconds(20) },
            new object[] {new DateTime(2004, 1, 11, 12, 0, 0), new DateTime(2004, 1, 12, 12, 0, 20), TimeSpan.FromSeconds(20).Add(TimeSpan.FromDays(1)) }
        };

    [Theory, MemberData(nameof(TimeStepData))]
    public void TestTimeStepConstructor(DateTime start, DateTime end, TimeSpan duration)
    {
        var fromTo = new TimeStep(start, end);
        Assert.Equal(start, fromTo.Start);
        Assert.Equal(end, fromTo.End);
        Assert.Equal(duration, fromTo.Duration);
        var fromDuration = new TimeStep(start, duration);
        Assert.Equal(start, fromDuration.Start);
        Assert.Equal(end, fromDuration.End);
        Assert.Equal(duration, fromDuration.Duration);
    }

    [Theory, MemberData(nameof(TimeStepData))]
    public void TestTimeStepNext(DateTime start, DateTime end, TimeSpan duration)
    {
        var test = new TimeStep(start, end);
        var next = test.Next(duration);
        Assert.Equal(end, next.Start);
        Assert.Equal(duration, next.Duration);
        Assert.Equal(end + duration, next.End);
    }
}
