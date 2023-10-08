using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Units
{
    public class KWH
    {
        public double Amount { get; private set; }

        public KWH(double amount) { Amount = amount; }

        public KWH(KW kW, TimeSpan time) : this(kW.Amount * time.TotalHours)
        {
        }

        public static readonly KWH Infinity = new(double.PositiveInfinity);
        public static readonly KWH Zero = new(0);


        public static KWH Max(KWH left, KWH right)
        {
            if(left.Amount >= right.Amount)
            {
                return left;
            }
            return right;
        }

        public static KWH Min(KWH left, KWH right)
        {
            if(left.Amount <= right.Amount)
            {
                return left;
            }
            return right;
        }

        public static KWH operator -(KWH left, KWH right)
        {
            return new(left.Amount - right.Amount);
        }

        public static KWH operator +(KWH left, KWH right)
        {
            return new(left.Amount + right.Amount);
        }

        public override string ToString()
        {
            var formatter = CultureInfo.CreateSpecificCulture("de-DE");
            formatter.NumberFormat.NumberDecimalDigits = 2;
            return Amount.ToString(formatter) + " KWH";
        }
    }
}
