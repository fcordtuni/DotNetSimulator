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

        public KWH(double amount) { this.Amount = amount; }

        public KWH(KW kW, TimeSpan time) : this(kW.Amount * time.TotalHours)
        {
        }
    }
}
