using DotNetSimulator.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Simulator.Elements
{
    internal class SolarPanel : ISimulationElement
    {
        private KW maxProduction;
        private KWH stepProduction;
        private readonly string name;

        public string Name { get => "Solar Pannel " + name; }

        public SolarPanel(KW maxProduction, string name) 
        {
            this.maxProduction = maxProduction;
            stepProduction = KWH.Zero;
            this.name = name;
        }

        public KWH GetProduction(KWH maxAmount)
        {
            KWH providablePower = KWH.Min(maxAmount, stepProduction);

            stepProduction -= providablePower;
            return providablePower;
        }

        private static double GetTotalProductionForTimeOfDay(TimeSpan timeOfDay)
        {
            //considering a sinuoid production curve, we can estimate the production as a cosine centered around 12 o'clock
            //for now: cosine[-pi, +pi] = cosine[0:00, 24:00]
            //talking about total production, we can use the integral(sin(x)), cos(x)
            //but, since we only care about the [-pi/2, pi/2] part (i.e. the positive cosine part), we define the function as follows:
            // P(t) := (sine(t) + 1) / 2 if t € [-pi/2, pi/2], 0 if t € [-pi, -pi/2[, 1 else

            double timeTransferred = ((double)timeOfDay.Ticks / TimeSpan.TicksPerDay - 0.5) * 2 * Math.PI;
            if(timeTransferred < -Math.PI / 2)
            {
                return 0;
            }
            else if(timeTransferred > Math.PI / 2)
            {
                return 1;
            }
            return (Math.Sin(timeTransferred) + 1) / 2;
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            //resetting current Production
            stepProduction = (GetTotalProductionForTimeOfDay(step.End.TimeOfDay) - GetTotalProductionForTimeOfDay(step.Start.TimeOfDay)) * maxProduction * step.Duration;
        }
    }
}
