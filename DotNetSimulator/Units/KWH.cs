using System.Globalization;

namespace DotNetSimulator.Units
{
    public class KWH
    {
        public double Amount { get; }

        public KWH(double amount) { Amount = amount; }

        public KWH(KW kW, TimeSpan time) : this(kW.Amount * time.TotalHours)
        {
        }

        public static readonly KWH Infinity = new(double.PositiveInfinity);
        public static readonly KWH Zero = new(0);


        public static KWH Max(KWH left, KWH right)
        {
            return left.Amount >= right.Amount ? left : right;
        }

        public static KWH Min(KWH left, KWH right)
        {
            return left.Amount <= right.Amount ? left : right;
        }

        public static KWH operator -(KWH left, KWH right)
        {
            return new KWH(left.Amount - right.Amount);
        }

        public static KWH operator +(KWH left, KWH right)
        {
            return new KWH(left.Amount + right.Amount);
        }

        public override string ToString()
        {
            var formatter = CultureInfo.CreateSpecificCulture("de-DE");
            return Amount.ToString(formatter) + " KWH";
        }
    }
}
