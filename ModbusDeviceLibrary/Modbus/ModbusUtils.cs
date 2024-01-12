//Author: FCORDT

using EasyModbus;

namespace ModbusDeviceLibrary.Modbus
{
    /// <summary>
    /// Utils for writing and reading to registers
    /// </summary>
    public static class ModbusUtils
    {
        /// <summary>
        /// Writes the given integer to the holding register. Takes sizeof(int) number of array elements
        /// </summary>
        /// <param name="holdingRegister">a Span pointing to the position of the holding register the int should be written to</param>
        /// <param name="value"></param>
        public static void WriteHoldingRegister(Span<short> holdingRegister, int value)
        {
            var convertedValue = ModbusClient.ConvertIntToRegisters(value);


            for (var i = 0; i < convertedValue.Length; i++)
            {
                holdingRegister[i] = (short)convertedValue[i];
            }
        }

        /// <summary>
        /// Writes the given string to the holding register. Takes sizeof(char) * value.Length number of array elements.
        /// If the given span is too short, the string will be shortened, else the remaining space will be filled with spaces
        /// </summary>
        /// <param name="holdingRegister">a Span pointing to the position of the holding register the int should be written to</param>
        /// <param name="value"></param>
        public static void WriteHoldingRegister(Span<short> holdingRegister, string value)
        {
            var strLen = holdingRegister.Length * 2;
            value = value.PadRight(strLen)[..strLen];
            var strRegisters = ModbusClient.ConvertStringToRegisters(value);
            for (var i = 0; i < strRegisters.Length; i++)
            {
                holdingRegister[i] = (short)strRegisters[i];
            }
        }

        /// <summary>
        /// Reads a String out of the given holding register, inverse of <see cref="WriteHoldingRegister(System.Span{short},string)"/>
        /// </summary>
        /// <param name="holdingRegister"></param>
        /// <returns></returns>
        public static string ReadHoldingRegisterStr(Span<short> holdingRegister)
        {
            var strLen = holdingRegister.Length * 2;
            var strData = new int[holdingRegister.Length];
            for (var i = 0; i < strData.Length; i++)
            {
                strData[i] = holdingRegister[i];
            }

            return ModbusClient.ConvertRegistersToString(strData, 0, strLen);
        }

        /// <summary>
        /// Reads a Integer out of the given holding register, inverse of <see cref="WriteHoldingRegister(System.Span{short},int)"/>
        /// </summary>
        /// <param name="holdingRegister"></param>
        /// <returns></returns>
        public static int ReadHoldingRegisterInt(Span<short> holdingRegister)
        {
            var registers = new int[]{ holdingRegister[0], holdingRegister[1] };
            return ModbusClient.ConvertRegistersToInt(registers);
        }
    }
}
