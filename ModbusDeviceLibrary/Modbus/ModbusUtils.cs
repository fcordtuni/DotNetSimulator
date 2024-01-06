//Author: FCORDT

using System.Text;

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
            var bytes = BitConverter.GetBytes(value);

            for (var i = 0; i < bytes.Length; i++)
            {
                holdingRegister[i] = bytes[i];
            }
        }

        /// <summary>
        /// Writes the given string to the holding register. Takes sizeof(char) * value.Length number of array elements.
        /// If the given span is too short, the string will be shortened, else the remaining space will be filled with zeroes
        /// </summary>
        /// <param name="holdingRegister">a Span pointing to the position of the holding register the int should be written to</param>
        /// <param name="value"></param>
        public static void WriteHoldingRegister(Span<short> holdingRegister, string value)
        {
            var strPos = 0;
            var hrPos = 0;

            //each character has sizeof(char) bytes, write sizeof(char) bytes to the holding register with each iteration
            while ((hrPos + sizeof(char)) <= holdingRegister.Length && strPos < value.Length)
            {
                var strBytes = BitConverter.GetBytes(value[strPos++]);
                foreach (var b in strBytes)
                {
                    holdingRegister[hrPos++] = b;
                }
            }

            //set remaining bytes to zero
            for (var i = hrPos; i < holdingRegister.Length; i++)
            {
                holdingRegister[i] = 0;
            }
        }

        /// <summary>
        /// Reads a String out of the given holding register, inverse of <see cref="WriteHoldingRegister(System.Span{short},string)"/>
        /// </summary>
        /// <param name="holdingRegister"></param>
        /// <returns></returns>
        public static string ReadHoldingRegisterStr(Span<short> holdingRegister)
        {
            var rVal = new StringBuilder(holdingRegister.Length / sizeof(char));
            var bytes = new byte[holdingRegister.Length];
            for (var i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = (byte)holdingRegister[i];
            }

            for (var i = 0; (i + sizeof(char)) <= bytes.Length; i += sizeof(char))
            {
                var c = BitConverter.ToChar(bytes.AsSpan()[i..]);
                if (c == 0)
                {
                    break;
                }
                rVal.Append(c);
            }
            return rVal.ToString();
        }

        /// <summary>
        /// Reads a Integer out of the given holding register, inverse of <see cref="WriteHoldingRegister(System.Span{short},int)"/>
        /// </summary>
        /// <param name="holdingRegister"></param>
        /// <returns></returns>
        public static int ReadHoldingRegisterInt(Span<short> holdingRegister)
        {
            var bytes = new byte[holdingRegister.Length];
            for (var i = 0; i < holdingRegister.Length; i++)
            {
                bytes[i] = (byte)holdingRegister[i];
            }
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
