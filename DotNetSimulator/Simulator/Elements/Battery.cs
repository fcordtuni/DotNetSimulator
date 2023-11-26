//Author: FCORDT
using DotNetSimulator.Units;
using NLog;
using System.Runtime.CompilerServices;
using ILogger = NLog.ILogger;
using KWH = DotNetSimulator.Units.KWH;

namespace DotNetSimulator.Simulator.Elements;

/// <summary>
/// This class simulates a Battery
/// </summary>
internal class Battery : ISimulationElement
{
    private readonly KWH _capacity;
    private readonly KW _maximumInput;
    private readonly KW _maximumOutput;
    private KWH _currentStepMaximumOutput;
    private KWH _currentStepOutput;
    public KWH CurrentChargeLevel { get; private set; }
    private readonly string _serial;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serial">Unique identifier</param>
    /// <param name="capacity"></param>
    /// <param name="maximumOutput"></param>
    /// <param name="maximumInput"></param>
    public Battery(string serial, KWH capacity, KW maximumOutput, KW maximumInput) : this(serial, capacity, maximumOutput, maximumInput, KWH.Zero)
    {
    }

    public Battery(string serial, KWH capacity, KW maximumOutput, KW maximumInput, KWH initialCharge)
    {
        _maximumOutput = maximumOutput;
        _maximumInput = maximumInput;
        _capacity = capacity;
        CurrentChargeLevel = KWH.Min(initialCharge, capacity);
        _currentStepMaximumOutput = KWH.Zero;
        _currentStepOutput = KWH.Zero;
        _serial = serial;
        Logger.Info("{this}: Created Battery with capacity of {initialCharge} / {capacity} and maximum IO of {input} / {output}", 
            this, CurrentChargeLevel, _capacity, _maximumInput, _maximumOutput);
    }

    private KWH CalculateMaximumOutput(KWH maxAmount)
    {
        var remainingProvidablePower = _currentStepMaximumOutput - _currentStepOutput;
        var providablePower = KWH.Min(remainingProvidablePower, CurrentChargeLevel);
        return KWH.Min(providablePower, maxAmount);
    }

    /// <inheritdoc />
    public KWH GetProduction(KWH maxAmount)
    {
        var providablePower = CalculateMaximumOutput(maxAmount);
        CurrentChargeLevel -= providablePower;
        _currentStepOutput += providablePower;
        Logger.Debug("{this}: Providing {amount}", this, providablePower);
        return providablePower;
    }

    private KWH CalculateMaximumInput(TimeStep step)
    {
        var maximumStepInput = _maximumInput * step.Duration;
        var remainingCapacity = _capacity - CurrentChargeLevel;
        return KWH.Min(remainingCapacity, maximumStepInput);
    }

    /// <inheritdoc />
    public void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> producers)
    {
        var maximumInput = CalculateMaximumInput(step);
        var totalInput = producers.Aggregate(KWH.Zero, (current, producer) => current + producer.GetProduction(maximumInput - current));
        Logger.Debug("{this}: Consuming {amount}", this, totalInput);
        CurrentChargeLevel += totalInput;
        _currentStepMaximumOutput = _maximumOutput * step.Duration;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "Battery " + _serial;
    }
}
