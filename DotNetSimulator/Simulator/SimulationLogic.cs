using DotNetSimulator.Simulator.Elements;
using DotNetSimulator.Units;
using DotNetSimulator.Utils;

namespace DotNetSimulator.Simulator
{
    internal class SimulationLogic
    {
        private readonly DAG<ISimulationElement> _powerGrid;
        public SimulationLogic() 
        { 
            _powerGrid = new DAG<ISimulationElement>();
            //for now: add some stuff here
            var sp1 = new LoggingDecorator(new SolarPanel(new KW(10), "1"));
            var sp2 = new LoggingDecorator(new SolarPanel(new KW(14), "2"));
            var sp3 = new LoggingDecorator(new SolarPanel(new KW(7), "3"));
            var pc1 = new LoggingDecorator(new PowerConverter("1"));
            var bt1 = new LoggingDecorator(new Battery(new KWH(1000), "2"));
            _powerGrid.AddEdge(sp1, pc1);
            _powerGrid.AddEdge(sp2, pc1);
            _powerGrid.AddEdge(sp3, pc1);
            _powerGrid.AddEdge(pc1, bt1);
        }

        public void Simulate(DateTime from, DateTime to, TimeSpan stepSize)
        {
            var orderedNodes = _powerGrid.TopologicalSort();
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
            foreach(var currentNode in orderedNodes)
            {
                currentNode.SimulateStep(step, _powerGrid.IncomingNodes(currentNode));
            }
        }
    }
}
