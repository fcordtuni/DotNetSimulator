﻿//Author: FCORDT
using DotNetSimulator.Units;
using ModbusDeviceLibrary.Modbus;
using NLog;

namespace DotNetSimulator.Simulator.Elements;

/// <summary>
/// This Class simulates a Power Converter
/// </summary>
public class PowerConverter : ISimulationElement, IModbusDevice
{
    private KWH _stepProduction;
    private KWH _stepProvision;

    private readonly string _serial;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private IModbusMapper? _mapper;
    private TimeStep _currentTimeStep;

    private KWH StepProduction
    {
        get => _stepProduction;
        set
        {
            _stepProduction = value;
            if (_mapper != null)
            {
                ModbusUtils.WriteHoldingRegister(_mapper.GetHoldingRegisters(this)[16..20], (value / _currentTimeStep).Amount * 1000);
            }
        }
    }

    private KWH StepProvision
    {
        get => _stepProvision;
        set
        {
            _stepProvision = value;
            if (_mapper != null)
            {
                ModbusUtils.WriteHoldingRegister(_mapper.GetHoldingRegisters(this)[20..24], (value / _currentTimeStep).Amount * 1000);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serial"></param>
    public PowerConverter(string serial)
    {
        _stepProduction = KWH.Zero;
        _stepProvision = KWH.Zero;
        _serial = serial;
        Logger.Info("{this}: Created PowerConverter", this);
    }


    /// <inheritdoc />
    public KWH GetProduction(KWH maxAmount)
    {
        var providablePower = KWH.Min(maxAmount, StepProduction);
        StepProduction -= providablePower;
        StepProvision += providablePower;
        Logger.Debug("{this}: Providing {amount}", this, providablePower);
        return providablePower;
    }

    /// <inheritdoc />
    public void SimulateStep(TimeStep step, IEnumerable<ISimulationElement> producers)
    {
        if (_mapper != null && !_mapper.GetCoils(this)[0])
        {
            StepProduction = KWH.Zero;
            Logger.Debug("{this}: Device Disabled, no Production", this);
        }
        else
        {
            StepProduction = producers.Select(x => x.GetProduction()).Aggregate((x, y) => x + y);
            Logger.Debug("{this}: Consuming {amount}", this, StepProduction);
        }

        StepProvision = KWH.Zero;
        _currentTimeStep = step;

    }

    public override string ToString()
    {
        return "PowerConverter " + _serial;
    }

    /// <inheritdoc />
    public void Register(IModbusMapper mapper)
    {
        _mapper = mapper;
        _mapper.RegisterHoldingRegisters(this,
            new List<ModbusInterfaceDescriptor>
            {
                new(0, 16),
                new(16, 4),
                new(20, 4)
            });
        _mapper.RegisterCoils(this,
            new List<ModbusInterfaceDescriptor>
            {
                new(0, 1)
            });

        var holdingRegisters = _mapper.GetHoldingRegisters(this);
        ModbusUtils.WriteHoldingRegister(holdingRegisters[..16], _serial);
    }
}
