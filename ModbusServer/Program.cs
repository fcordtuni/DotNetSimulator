//Author: FCORDT

using DotNetSimulator.Simulator.Time;
using DotNetSimulator.Simulator;
using DotNetSimulator.Simulator.Elements;
using DotNetSimulator.Units;
using ModbusServer.Binding;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("starting setup");

//setting up the Simulator
SimulationLogic logic =
    new(ISimulationTimer.FastForward(DateTime.Now.AddDays(-1), DateTime.Now, TimeSpan.FromSeconds(10))
        .AndThen(ISimulationTimer.RealTime(1, TimeSpan.FromSeconds(1), DateTime.Now)));
var sp1 = new SolarPanel("SP1", new KW(0.2));
var sp2 = new SolarPanel("SP2", new KW(0.3));
var sp3 = new SolarPanel("SP3", new KW(0.17));
var pc = new PowerConverter("PC1");
var bt = new Battery("BT1", new KWH(150), new KW(0.50), new KW(0.1));

logic.AddLinks(new (ISimulationElement, ISimulationElement)[] { (sp1, pc), (sp2, pc), (sp3, pc), (pc, bt) });

var server = new EasyModbus.ModbusServer();
EasyModbusModbusMapper mapper = new(server);
sp1.Register(mapper); //0-19
sp2.Register(mapper); //20-39
sp3.Register(mapper); //40-59
pc.Register(mapper); //60-79
bt.Register(mapper); //80-103

var simulationTask = logic.RunSimulation();
server.Listen();
await simulationTask;