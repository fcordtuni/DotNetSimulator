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

var startTime = DateTime.Today + new TimeSpan(12, 0, 0);

//setting up the Simulator
SimulationLogic logic =
    new(ISimulationTimer.FastForward(startTime.AddDays(-1), startTime, TimeSpan.FromSeconds(10))
        .AndThen(ISimulationTimer.RealTime(1, TimeSpan.FromSeconds(1), startTime)));
var sp1 = new SolarPanel("SP1", new KW(0.2));
var sp2 = new SolarPanel("SP2", new KW(0.3));
var sp3 = new SolarPanel("SP3", new KW(0.17));
var pc = new PowerConverter("PC1");
var bt = new Battery("BT1", new KWH(150), new KW(0.50), new KW(0.1));

logic.AddLinks(new (ISimulationElement, ISimulationElement)[] { (sp1, pc), (sp2, pc), (sp3, pc), (pc, bt) });

var server = new EasyModbus.ModbusServer();
EasyModbusModbusMapper mapper = new(server);
sp1.Register(mapper); //0-23
sp2.Register(mapper); //24-47
sp3.Register(mapper); //48-71
pc.Register(mapper); //72-95
bt.Register(mapper); //96-128

var simulationTask = logic.RunSimulation();
server.Listen();
await simulationTask;