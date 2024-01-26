//Author: FCORDT

using DotNetSimulator.Units;
using ModbusDeviceLibrary.Modbus;
using NLog;

namespace DotNetSimulator.Simulator.Elements;

/// <summary>
/// Simulates a Solar Panel
/// </summary>
public class SolarPanel : ISimulationElement, IModbusDevice
{
    private readonly KW _maxProduction;
    private KWH _stepProduction;
    private readonly string _serial;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private IModbusMapper? _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serial"></param>
    /// <param name="maxProduction"></param>
    public SolarPanel(string serial, KW maxProduction)
    {
        _maxProduction = maxProduction;
        _stepProduction = KWH.Zero;
        _serial = serial;
        Logger.Info("{this}: Created Solar Panel with {maxProd} max production", this, maxProduction);
    }

    /// <inheritdoc />
    public KWH GetProduction(KWH maxAmount)
    {
        var providablePower = KWH.Min(maxAmount, _stepProduction);

        _stepProduction -= providablePower;
        Logger.Debug("{this}: Providing {amount}", this, _stepProduction);
        return providablePower;
    }

    /// <summary>
    /// Uses numerical integration (quadrature interpolation) in order to get the provided power
    ///
    /// For now the cos function (more specifically: 0 for x€[0, 0.25[, -cos(x * 2 * PI) for x€[0.25,0.75], 1 for x € ]0.75, 1]) is taken as a provider
    /// todo: later on we need to provide some variation, i.e. weather, etc...
    /// </summary>
    /// <param name="step">the interval to be integrated</param>
    /// <param name="maxProduction">the multiplicative scaling factor</param>
    /// <returns>the amount of KWH produced</returns>
    private static KWH GetProductionForTimeStep(TimeStep step, KW maxProduction)
    {
        const double twoPi = double.Pi * 2;
        var minTime = TimeSpan.FromHours(6);
        var maxTime = TimeSpan.FromHours(18);
        if (step.End.TimeOfDay < minTime)
        {
            return KWH.Zero;
        }

        if (step.Start.TimeOfDay > maxTime)
        {
            return KWH.One * maxProduction.Amount;
        }

        step = TimeStep.Clamp(step, minTime, maxTime);
        var productionPercentage = (-Math.Cos(step.Start.TimeOfDay.TotalHours * twoPi) -
                                    Math.Cos(step.End.TimeOfDay.TotalHours * twoPi)) / 2;
        return productionPercentage * maxProduction * step.Duration;
    }

    /// <inheritdoc />
    public void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> producers)
    {
        //resetting current Production
        _stepProduction = GetProductionForTimeStep(step, _maxProduction);
        if (_mapper != null)
        {
            ModbusUtils.WriteHoldingRegister(_mapper.GetHoldingRegisters(this)[18..20], (int)((_stepProduction / step).Amount * 1000));
        }
        Logger.Debug("{this}: Producing {amount} Watt", this, (_stepProduction / step).Amount * 1000);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "Solar Panel " + _serial;
    }

    /// <inheritdoc />
    public void Register(IModbusMapper mapper)
    {
        _mapper = mapper;
        _mapper.RegisterHoldingRegisters(this,
            new List<ModbusInterfaceDescriptor>
            {
                new(0, 16, "Serial Number"),
                new(16, 2, "Max Production in Watt"),
                new(18, 2, "Current Production in Watt")
            });

        var holdingRegisters = _mapper.GetHoldingRegisters(this);
        ModbusUtils.WriteHoldingRegister(holdingRegisters[..16], _serial);

        ModbusUtils.WriteHoldingRegister(holdingRegisters[16..18], (int)(_maxProduction.Amount * 1000));
    }
}
