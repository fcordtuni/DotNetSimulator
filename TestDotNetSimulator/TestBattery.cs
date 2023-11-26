//Author: FCORDT
using DotNetSimulator.Simulator;
using DotNetSimulator.Simulator.Elements;
using DotNetSimulator.Units;

namespace TestDotNetSimulator;
public class TestBattery
{
    public static IEnumerable<object[]> ProducedBatteryData() =>
        new List<object[]>
        {
            new object[] {new KW(100.0), new TimeStep(DateTime.Now, DateTime.Now.AddDays(1))},
            new object[] {new KW(150.3), new TimeStep(DateTime.Now, DateTime.Now.AddSeconds(120))},
        };

    public static IEnumerable<object[]> BatteryConstructorData() =>
        new List<object[]>
        {
            new object[] { new KWH(150), new KWH(245), new KWH(150) },
            new object[] { new KWH(350), new KWH(245), new KWH(245) },
        };

    [Theory, MemberData(nameof(ProducedBatteryData))]
    public void TestBatteryOnlyTakesMaximumAmountIn(KW maxInput, TimeStep timeStep)
    {
        var infinityProducer = TestSimulationElements.InfiniteProducer();
        var battery = new Battery("", KWH.Infinity, KW.Zero, maxInput);
        battery.SimulateStep(timeStep, new List<ISimulationElement> { infinityProducer });
        Assert.Equal(maxInput * timeStep.Duration, infinityProducer.TotalOutgoing());
    }

    [Theory, MemberData(nameof(BatteryConstructorData))]
    public void TestBatteryInitialCharge(KWH capacity, KWH initialCharge, KWH expectedCharge)
    {
        var battery = new Battery("", capacity, KW.Infinity, KW.Infinity, initialCharge);
        Assert.Equal(expectedCharge, battery.CurrentChargeLevel);

    }

    [Theory, MemberData(nameof(ProducedBatteryData))]
    public void TestBatteryOnlyProvidesDefinedAmount(KW maxOutput, TimeStep timeStep)
    {
        var infiniteConsumer = TestSimulationElements.InfiniteConsumer();
        var battery = new Battery("", KWH.Infinity, maxOutput, KW.Zero, KWH.Infinity);
        infiniteConsumer.SimulateStep(timeStep, new List<ISimulationElement> { battery });
        Assert.Equal(maxOutput * timeStep.Duration, infiniteConsumer.TotalIncoming());
    }
}
