//Author: FCORDT
using DotNetSimulator.Units;
using NLog;
using ILogger = NLog.ILogger;

namespace DotNetSimulator.Simulator.Elements
{
    internal class SolarPanel : ISimulationElement
    {
        private readonly KW _maxProduction;
        private KWH _stepProduction;
        private readonly string _serial;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public SolarPanel(string serial, KW maxProduction) 
        {
            _maxProduction = maxProduction;
            _stepProduction = KWH.Zero;
            _serial = serial;
            Logger.Info("{this}: Created Solar Panel with {maxProd} max production", this, maxProduction);
        }

        public KWH GetProduction(KWH maxAmount)
        {
            var providablePower = KWH.Min(maxAmount, _stepProduction);

            _stepProduction -= providablePower;
            Logger.Debug("{this}: Providing {amount}", this, _stepProduction);
            return providablePower;
        }

        private static double GetTotalProductionForTimeOfDay(TimeSpan timeOfDay)
        {
            //considering a sinusoid production curve, we can estimate the production as a cosine centered around 12 o'clock
            //for now: cosine[-pi, +pi] = cosine[0:00, 24:00]
            //talking about total production, we can use the integral(sin(x)), cos(x)
            //but, since we only care about the [-pi/2, pi/2] part (i.e. the positive cosine part), we define the function as follows:
            // P(t) := (sine(t) + 1) / 2 if t € [-pi/2, pi/2], 0 if t € [-pi, -pi/2[, 1 else

            var timeTransferred = ((double)timeOfDay.Ticks / TimeSpan.TicksPerDay - 0.5) * 2 * Math.PI;
            return timeTransferred switch
            {
                < -Math.PI / 2 => 0,
                > Math.PI / 2 => 1,
                _ => (Math.Sin(timeTransferred) + 1) / 2
            };
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            //resetting current Production
            _stepProduction = (GetTotalProductionForTimeOfDay(step.End.TimeOfDay) - GetTotalProductionForTimeOfDay(step.Start.TimeOfDay)) * _maxProduction * step.Duration;
            Logger.Debug("{this}: Producing {amount}", this, _stepProduction);
        }

        public override string ToString()
        {
            return "Solar Panel " + _serial;
        }
    }
}
