//Author: FCORDT
using DotNetSimulator.Simulator;
using DotNetSimulator.Simulator.Elements;
using DotNetSimulator.Units;
using NLog;
using NLog.Web;

LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

SimulationLogic logic = new();
var sp1 = new SolarPanel("SP1", new KW(0.2));
var sp2 = new SolarPanel("SP2", new KW(0.3));
var sp3 = new SolarPanel("SP3", new KW(0.17));
var pc = new PowerConverter("PC1");
var bt = new Battery("BT1", new KWH(150), new KW(0.50), new KW(0.1));

logic.AddLinks(new(ISimulationElement, ISimulationElement)[] {(sp1, pc), (sp2, pc), (sp3, pc), (pc, bt)}) ;
logic.RealTimeSimulation(DateTime.Now.AddHours(-1), TimeSpan.FromSeconds(1));
