//Author: FCORDT

using ModbusDeviceLibrary.Modbus;

namespace TestModbusDeviceLibrary
{
    public class ModbusUtilsTests
    {
        [Theory, InlineData(4), InlineData(int.MaxValue), InlineData(-1350), InlineData(int.MinValue)]
        public void TestWriteHoldingRegisterIntValue(int value)
        {
            // Arrange
            Span<short> holdingRegister = new short[4];

            // Act
            ModbusUtils.WriteHoldingRegister(holdingRegister, value);

            // Assert
            var result = ModbusUtils.ReadHoldingRegisterInt(holdingRegister);
            Assert.Equal(value, result);
        }

        [Theory, InlineData("Hello World", 150, "Hello World"), InlineData("Hello World", 3, "Hello")]
        public void WriteHoldingRegister_StringValue_Success(string value, int spanLength, string expectedValue)
        {
            // Arrange
            Span<short> holdingRegister = new short[spanLength];

            // Act
            ModbusUtils.WriteHoldingRegister(holdingRegister, value);

            // Assert
            var result = ModbusUtils.ReadHoldingRegisterStr(holdingRegister);
            Assert.Equal(expectedValue, result.TrimEnd());
        }
    }
}