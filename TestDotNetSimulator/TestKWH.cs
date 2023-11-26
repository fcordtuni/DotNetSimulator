//Author: FCORDT
using DotNetSimulator.Units;

namespace TestDotNetSimulator;
public class TestKWH
{
    [Theory, InlineData(1), InlineData(-3), InlineData(0), InlineData(0.344)]
    public void TestKwhContainsCorrectValues(double amount)
    {
        var testUnit = new KW(amount);
        Assert.Equal(amount, testUnit.Amount);
    }

    [Theory, InlineData(1, 1), InlineData(1, 2), InlineData(0.0001, 0.00001)]
    public void TestKwhEquals(double first, double second)
    {
        Assert.Equal(first.Equals(second), new KWH(first).Equals(new KWH(second)));
    }

    [Theory, InlineData(2, -1), InlineData(-3.2, 1.4), InlineData(0, 0)]
    public void TestKwhMaxMin(double first, double second)
    {
        Assert.Equal(KWH.Max(new KWH(first), new KWH(second)), new KWH(double.Max(first, second)));
        Assert.Equal(KWH.Max(new KWH(second), new KWH(first)), new KWH(double.Max(first, second)));
        Assert.Equal(KWH.Min(new KWH(first), new KWH(second)), new KWH(double.Min(first, second)));
        Assert.Equal(KWH.Min(new KWH(second), new KWH(first)), new KWH(double.Min(first, second)));
    }

    [Theory, InlineData(2, 1), InlineData(3, 1), InlineData(5, -3.1)]
    public void TestKwhOperations(double first, double second)
    {
        var dResult = first - second;
        var kwhResult = new KWH(first) - new KWH(second);
        Assert.Equal(dResult, kwhResult.Amount);
        dResult = first + second;
        kwhResult = new KWH(first) + new KWH(second);
        Assert.Equal(dResult, kwhResult.Amount);
    }

    public static IEnumerable<object[]> KwhKwTimespanTestData() =>
        new List<object[]>
        {
                new object[] { new KW(10.0), new TimeSpan(0, 1, 0, 0), new KWH(10.0) },
                new object[] { new KW(10.0), new TimeSpan(0, 0, 30, 0), new KWH(5.0) },
        };

    [Theory, MemberData(nameof(KwhKwTimespanTestData))]
    public void TestKwhFromKwTimespanConstructor(KW kw, TimeSpan timeSpan, KWH expectedResult)
    {
        Assert.Equal(expectedResult, new KWH(kw, timeSpan));
    }

    [Theory, MemberData(nameof(KwhKwTimespanTestData))]
    public void TestKwhFromKwTimespanKWMethod(KW kw, TimeSpan timeSpan, KWH expectedResult)
    {
        Assert.Equal(expectedResult, kw.ForTimeSpan(timeSpan));
    }


    [Theory, MemberData(nameof(KwhKwTimespanTestData))]
    public void TestKwhFromKwTimespanKWOperator(KW kw, TimeSpan timeSpan, KWH expectedResult)
    {
        Assert.Equal(expectedResult, kw * timeSpan);
    }
}