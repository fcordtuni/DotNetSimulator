﻿using DotNetSimulator.Simulator.Elements;
using DotNetSimulator.Units;
using DotNetSimulator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSimulator.Simulator
{
    internal class SimulationLogic
    {
        private readonly DAG<ISimulationElement> powerGrid;
        public SimulationLogic() 
        { 
            powerGrid = new DAG<ISimulationElement>();
            //for now: add some stuff here
            var sp1 = new LoggingDecorator(new SolarPanel(new KW(10), "1"));
            var sp2 = new LoggingDecorator(new SolarPanel(new KW(14), "2"));
            var sp3 = new LoggingDecorator(new SolarPanel(new KW(7), "3"));
            var pc1 = new LoggingDecorator(new PowerConverter("1"));
            var bt1 = new LoggingDecorator(new Battery(new KWH(1000), "2"));
            powerGrid.AddEdge(sp1, pc1);
            powerGrid.AddEdge(sp2, pc1);
            powerGrid.AddEdge(sp3, pc1);
            powerGrid.AddEdge(pc1, bt1);
        }

        public void Simulate(DateTime from, DateTime to, TimeSpan stepSize)
        {
            List<ISimulationElement> orderedNodes = powerGrid.TopologicalSort();
            TimeStep step = new(from, from + stepSize);
            while(step.Start < to)
            {
                Console.WriteLine("Simulating " + step);
                SimulateStep(step, orderedNodes);
                step = step.Next(stepSize);
            }

        }

        private void SimulateStep(TimeStep step, List<ISimulationElement> orderedNodes)
        {
            foreach(ISimulationElement currentNode in orderedNodes)
            {
                currentNode.SimulateStep(step, powerGrid.IncomingNodes(currentNode));
            }
        }
    }
}
