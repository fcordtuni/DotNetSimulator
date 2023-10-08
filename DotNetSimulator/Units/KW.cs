using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Units
{
    public struct KW
    {
        public double Amount { get; private set; }
        public KW(double amount) { Amount = amount; }


        public readonly KWH ForTimeSpan(TimeSpan time) => new(this, time);

        public static KWH operator *(KW kw, TimeSpan time) => kw.ForTimeSpan(time);

        public static KW operator *(KW left, double right) => new(left.Amount * right);
        public static KW operator *(double left, KW right) => right * left;
    }
}
