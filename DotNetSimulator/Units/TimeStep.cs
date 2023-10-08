﻿using System;
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
        public TimeStep(DateTime start, TimeSpan duration) { Start = start; End = start + duration; Duration = duration; }

        public TimeStep Next(TimeSpan duration) { return new TimeStep(End, duration); }
    }
}
