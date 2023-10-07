using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Units
{
    internal struct TimeStep
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public TimeSpan Duration { get; private set; }

        public TimeStep(DateTime start, DateTime end) { Start = start; End = end; Duration = end - start; }
    }
}
