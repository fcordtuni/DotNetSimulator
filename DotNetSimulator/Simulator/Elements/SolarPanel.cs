//Author: FCORDT

using DotNetSimulator.Units;
using ModbusDeviceLibrary.Modbus;
using NLog;

namespace DotNetSimulator.Simulator.Elements;

/// <summary>
/// Simulates a Solar Panel
/// </summary>
internal class SolarPanel : ISimulationElement, IModbusDevice
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
    /// will calculate the Production for a given TimeSpan like the following:
    /// 
    /// considering a sinusoid production curve, we can estimate the production as a cosine centered around 12 o'clock
    /// for now: cosine[-pi, +pi] = cosine[0:00, 24:00]
    /// talking about total production, we can use the integral(sin(x)), cos(x)
    /// but, since we only care about the [-pi/2, pi/2] part (i.e. the positive cosine part), we define the function as follows:
    /// P(t) := (sine(t) + 1) / 2 if t € [-pi/2, pi/2], 0 if t € [-pi, -pi/2[, 1 else
    /// </summary>
    /// <param name="timeOfDay"></param>
    /// <returns></returns>
    private static double GetTotalProductionForTimeOfDay(TimeSpan timeOfDay)
    {
        var timeTransferred = ((double)timeOfDay.Ticks / TimeSpan.TicksPerDay - 0.5) * 2 * Math.PI;
        return timeTransferred switch
        {
            < -Math.PI / 2 => 0,
            > Math.PI / 2 => 1,
            _ => (Math.Sin(timeTransferred) + 1) / 2
        };
    }

    /// <inheritdoc />
    public void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> producers)
    {
        //resetting current Production
        _stepProduction = (GetTotalProductionForTimeOfDay(step.End.TimeOfDay) - GetTotalProductionForTimeOfDay(step.Start.TimeOfDay)) * _maxProduction * step.Duration;
        if (_mapper != null) _mapper.GetHoldingRegisters(this)[17] = (short)(_stepProduction / step).Amount;
        Logger.Debug("{this}: Producing {amount}", this, _stepProduction);
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
                new(16, 1, "Max Production in Watt"),
                new(17, 1, "Current Production in Watt")
            });

        var holdingRegisters = _mapper.GetHoldingRegisters(this);

        //set serial
        for (var i = 0; i < 16 && i < _serial.Length; i++)
        {
            holdingRegisters[i] = (short)_serial[i];
        }

        holdingRegisters[16] = (short)(_maxProduction.Amount * 1000); //manually converting, ugh
    }
}
