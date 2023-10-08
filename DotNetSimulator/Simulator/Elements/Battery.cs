using DotNetSimulator.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Simulator.Elements
{
    internal class Battery : ISimulationElement
    {
        private readonly KWH capacity;
        private KWH currentLevel;
        private readonly String name;

        public Battery(KWH capacity, string name)
        {
            this.name = name;
            this.capacity = capacity;
            currentLevel = KWH.Zero;
        }

        public string Name { get => "Battery " + name; }

        public KWH GetProduction(KWH maxAmount)
        {
            KWH providablePower = KWH.Min(maxAmount, currentLevel);
            currentLevel -= providablePower;
            return providablePower;
        }

        public void SimulateStep(TimeStep step, ICollection<ISimulationElement> producers)
        {
            //for now: accept all power!
            foreach(var producer in producers)
            {
                currentLevel += producer.GetProduction(capacity - currentLevel);
            }
        }
    }
}
