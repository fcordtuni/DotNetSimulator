// See https://aka.ms/new-console-template for more information
using DotNetSimulator.Simulator;
using DotNetSimulator.Simulator.Elements;
using DotNetSimulator.Units;

SimulationLogic logic = new();
var sp1 = new LoggingDecorator(new SolarPanel(new KW(0.2), "1"));
var sp2 = new LoggingDecorator(new SolarPanel(new KW(0.3), "2"));
var sp3 = new LoggingDecorator(new SolarPanel(new KW(0.17), "3"));
var pc = new LoggingDecorator(new PowerConverter("1"));
var bt = new LoggingDecorator(new Battery(new KWH(150), "1", new KW(0.50), new KW(0.1)));

logic.AddLinks(new(ISimulationElement, ISimulationElement)[] {(sp1, pc), (sp2, pc), (sp3, pc), (pc, bt)}) ;
logic.RealTimeSimulation(DateTime.Now.AddHours(-1), TimeSpan.FromSeconds(1));
