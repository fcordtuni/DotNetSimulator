// Autor: Elisabeth Gisser
using System;
using System.Collections.Generic;

namespace ModBusServer
{
    /// <summary>
    /// Class representing Modbus registers and implementing the IDeviceModbus interface
    /// </summary>
    public class ModbusRegisters : IDeviceModbus
    {
        private readonly Dictionary<ModbusRegistersAddresses, int> _registers = new Dictionary<ModbusRegistersAddresses, int>();

        /// <summary>
        /// Constructor for ModbusRegisters class
        /// </summary>
        public ModbusRegisters()
        {
            InitializeRegisters();
        }

        /// <summary>
        /// Initializes registers with default values
        /// </summary>
        private void InitializeRegisters()
        {
            foreach (ModbusRegistersAddresses address in Enum.GetValues(typeof(ModbusRegistersAddresses)))
            {
                _registers[address] = 0;
            }
        }

        /// <summary>
        /// Retrieves the value associated with a Modbus register address
        /// </summary>
        /// <param name="address">The Modbus register address</param>
        /// <returns>The value stored at the specified address</returns>
        public int GetValueByAddress(ModbusRegistersAddresses address)
        {
            if (_registers.ContainsKey(address))
            {
                return _registers[address];
            }

            Console.WriteLine($"Register address {address} not found.");
            throw new ArgumentException("Registeradresse ungültig");
        }

        /// <summary>
        /// Sets the value for a specific Modbus register address
        /// </summary>
        /// <param name="address">The Modbus register address</param>
        /// <param name="value">The value to be set</param>
        public void SetValueByAddress(ModbusRegistersAddresses address, int value)
        {
            if (_registers.ContainsKey(address))
            {
                _registers[address] = value;
                Console.WriteLine($"Register address {address} set to value: {value}");
            }
            else
            {
                Console.WriteLine($"Register address {address} not found.");
            }
        }
    }
}