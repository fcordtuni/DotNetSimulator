using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Units
{
    public enum Current
    {
        DC,
        AC
    }
    public struct KW
    {
        public double Amount { get; private set; }
        public Current Current { get; private set; }
        public KW(double amount, Current current) { this.Amount = amount; this.Current = current; }


        public KWH forTimeSpan(TimeSpan time)
        {
            return new KWH(this, time);
        }

        public static KWH operator *(KW kw, TimeSpan time)
        {
            return kw.forTimeSpan(time);
        }
    }
}
