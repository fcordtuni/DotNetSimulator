using System.Runtime.InteropServices;
using DotNetSimulator.Units;

namespace TestDotNetSimulator
{
    public class TestKWH
    {
        [Theory]
        [InlineData(1), InlineData(-3), InlineData(0), InlineData(0.344)]
        public void TestKwhContainsCorrectValues(double amount)
        {
            var testUnit = new KW(amount);
            Assert.Equal(amount, testUnit.Amount);
        }

        [Theory]
        [InlineData(1, 1), InlineData(1, 2), InlineData(0.0001, 0.00001)]
        public void TestKwhEquals(double first, double second)
        {
            Assert.Equal(first.Equals(second), new KWH(first).Equals(new KWH(second)));
        }

        [Theory]
        [InlineData(2, -1), InlineData(-3.2, 1.4), InlineData(0, 0)]
        public void TestKwhMax(double first, double second)
        {
            Assert.Equal(KWH.Max(new KWH(first), new KWH(second)), new KWH(double.Max(first, second)));
            Assert.Equal(KWH.Max(new KWH(second), new KWH(first)), new KWH(double.Max(first, second)));
        }

        [Theory]
        [InlineData(2, 1), InlineData(3, 1), InlineData(5, -3.1)]
        public void TestKwhOperations(double first, double second)
        {
            var dResult = first - second;
            var kwhResult = new KWH(first) - new KWH(second);
            Assert.Equal(dResult, kwhResult.Amount);
            dResult = first + second;
            kwhResult = new KWH(first) + new KWH(second);
            Assert.Equal(dResult, kwhResult.Amount);
        }
    }
}