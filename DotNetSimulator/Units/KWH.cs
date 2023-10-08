using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Units
{
    public class KWH
    {
        public double Amount { get; private set; }
        public Current Current { get; private set; }

        public KWH(double amount, Current current) { this.Amount = amount; this.Current = current; }

        public KWH(KW kW, TimeSpan time) : this(kW.Amount * time.TotalHours, kW.Current)
        {
        }
    }
}
