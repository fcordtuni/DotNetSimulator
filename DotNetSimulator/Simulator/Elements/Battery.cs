//Author: FCORDT
using DotNetSimulator.Units;
using ModbusDeviceLibrary.Modbus;
using NLog;
using KWH = DotNetSimulator.Units.KWH;

namespace DotNetSimulator.Simulator.Elements;

/// <summary>
/// This class simulates a Battery
/// </summary>
public class Battery : ISimulationElement, IModbusDevice
{
    private readonly KWH _capacity;
    private readonly KW _maximumInput;
    private readonly KW _maximumOutput;
    private KWH _currentStepMaximumOutput;
    private KWH _currentStepOutput;
    private KWH _currentChargeLevel;

    public KWH CurrentChargeLevel
    {
        get => _currentChargeLevel;
        private set
        {
            _currentChargeLevel = value;
            if (_mapper != null)
            {
                ModbusUtils.WriteHoldingRegister(_mapper.GetHoldingRegisters(this)[18..20], (int)(value.Amount * 1000 * 3600));
            }
        }
    }

    private readonly string _serial;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private IModbusMapper? _mapper;

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
        _currentChargeLevel = KWH.Min(initialCharge, capacity);
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

    /// <inheritdoc />
    public void Register(IModbusMapper mapper)
    {
        _mapper = mapper;
        _mapper.RegisterHoldingRegisters(this,
            new List<ModbusInterfaceDescriptor>
            {
                new(0, 16, "Serial Number"),
                new(16, 2, "Max Capacity in watt-seconds"),
                new(18, 2, "Current Capacity in watt-seconds"),
                new(20, 2, "Maximum Input in Watt"),
                new(22, 2, "Maximum Output in Watt")
            });

        var holdingRegisters = _mapper.GetHoldingRegisters(this);
        ModbusUtils.WriteHoldingRegister(holdingRegisters[..16], _serial);
        ModbusUtils.WriteHoldingRegister(holdingRegisters[16..18], (int)(_capacity.Amount * 1000 * 3600));
        ModbusUtils.WriteHoldingRegister(holdingRegisters[20..22], (int)(_maximumInput.Amount * 1000));
        ModbusUtils.WriteHoldingRegister(holdingRegisters[22..24], (int)(_maximumOutput.Amount * 1000));
    }
}
