//Author: FCORDT
using DotNetSimulator.Units;
using NLog;
using ILogger = NLog.ILogger;

namespace DotNetSimulator.Simulator.Elements;
internal class Battery : ISimulationElement
{
    private readonly KWH _capacity;
    private readonly KW _maximumInput;
    private readonly KW _maximumOutput;
    private KWH _currentStepMaximumOutput;
    private KWH _currentStepOutput;
    private KWH _currentChargeLevel;
    private readonly string _serial;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public Battery(string serial, KWH capacity, KW maximumOutput, KW maximumInput)
    {
        _maximumOutput = maximumOutput;
        _maximumInput = maximumInput;
        _capacity = capacity;
        _currentChargeLevel = KWH.Zero;
        _currentStepMaximumOutput = KWH.Zero;
        _currentStepOutput = KWH.Zero;
        _serial = serial;
        Logger.Info("{this}: Created Battery with capacity of {capacity} and maximum IO of {input} / {output}", this, capacity, maximumInput, maximumOutput);
    }

    private KWH CalculateMaximumOutput(KWH maxAmount)
    {
        var remainingProvidablePower = _currentStepMaximumOutput - _currentStepOutput;
        var providablePower = KWH.Min(remainingProvidablePower, _currentChargeLevel);
        return KWH.Min(providablePower, maxAmount);
    }

    public KWH GetProduction(KWH maxAmount)
    {
        var providablePower = CalculateMaximumOutput(maxAmount);
        _currentChargeLevel -= providablePower;
        _currentStepOutput += providablePower;
        Logger.Debug("{this}: Providing {amount}", this, providablePower);
        return providablePower;
    }

    private KWH CalculateMaximumInput(TimeStep step)
    {
        var maximumStepInput = _maximumInput * step.Duration;
        var remainingCapacity = _capacity - _currentChargeLevel;
        return KWH.Min(remainingCapacity, maximumStepInput);
    }

    public void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> producers)
    {
        var maximumInput = CalculateMaximumInput(step);
        var totalInput = producers.Aggregate(KWH.Zero, (current, producer) => current + producer.GetProduction(maximumInput - current));
        Logger.Debug("{this}: Consuming {amount}", this, totalInput);
        _currentChargeLevel += totalInput;
        _currentStepMaximumOutput = _maximumOutput * step.Duration;
    }

    public override string ToString()
    {
        return "Battery " + _serial;
    }
}
