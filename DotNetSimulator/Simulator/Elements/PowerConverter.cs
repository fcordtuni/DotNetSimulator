//Author: FCORDT
using DotNetSimulator.Units;
using NLog;
using ILogger = NLog.ILogger;

namespace DotNetSimulator.Simulator.Elements;

/// <summary>
/// This Class simulates a Power Converter
/// </summary>
internal class PowerConverter : ISimulationElement
{
    private KWH _stepProduction;
    private readonly string _serial;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serial"></param>
    public PowerConverter(string serial)
    {
        _stepProduction = KWH.Zero;
        _serial = serial;
        Logger.Info("{this}: Created PowerConverter", this);
    }


    /// <inheritdoc />
    public KWH GetProduction(KWH maxAmount)
    {
        var providablePower = KWH.Min(maxAmount, _stepProduction);
        _stepProduction -= providablePower;
        Logger.Debug("{this}: Providing {amount}", this, providablePower);
        return providablePower;
    }

    /// <inheritdoc />
    public void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> producers)
    {
        _stepProduction = producers.Select(x => x.GetProduction()).Aggregate((x, y) => x + y);
        Logger.Debug("{this}: Consuming {amount}", this, _stepProduction);
    }

    public override string ToString()
    {
        return "PowerConverter " + _serial;
    }
}
