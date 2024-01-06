//Author: FCORDT
using DotNetSimulator.Units;

namespace TestDotNetSimulator;
public class TestKW
{
    [Theory, InlineData(1), InlineData(2), InlineData(-3), InlineData(0.001)]
    public void TestKWInputReturns(double input)
    {
        Assert.Equal(input, new KW(input).Amount);
    }

    [Theory, InlineData(1, 1), InlineData(1, 2), InlineData(0.0001, 0.00001)]
    public void TestKwEquals(double first, double second)
    {
        Assert.Equal(first.Equals(second), new KW(first).Equals(new KW(second)));
    }

    public static TheoryData<KW, double, KW> KwMultData() => 
        new()
        {
            { new KW(1), 1, new KW(1) },
            { new KW(1.123), 2.4, new KW(2.6952) },
            { new KW(2.4), -1.32, new KW(-3.168) }
        };

    [Theory, MemberData(nameof(KwMultData))]
    public void TestKWMultiplication(KW input, double multiplier, KW expectedOutput)
    {
        Assert.Equal(expectedOutput, input * multiplier);
        Assert.Equal(expectedOutput, multiplier * input);
    }
}

