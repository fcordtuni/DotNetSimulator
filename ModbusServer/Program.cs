//Author: FCORDT

using DotNetSimulator.Simulator.Time;
using DotNetSimulator.Simulator;
using DotNetSimulator.Simulator.Elements;
using DotNetSimulator.Units;
using ModbusServer.Binding;

//setting up the Simulator
SimulationLogic logic =
    new(ISimulationTimer.FastForward(DateTime.Now.AddDays(-1), DateTime.Now.AddHours(-5), TimeSpan.FromSeconds(10))
        .AndThen(ISimulationTimer.RealTime(1, TimeSpan.FromSeconds(1), DateTime.Now.AddHours(-5))));
var sp1 = new SolarPanel("SP1", new KW(0.2));
var sp2 = new SolarPanel("SP2", new KW(0.3));
var sp3 = new SolarPanel("SP3", new KW(0.17));
var pc = new PowerConverter("PC1");
var bt = new Battery("BT1", new KWH(150), new KW(0.50), new KW(0.1));

logic.AddLinks(new (ISimulationElement, ISimulationElement)[] { (sp1, pc), (sp2, pc), (sp3, pc), (pc, bt) });

var server = new EasyModbus.ModbusServer();
EasyModbusModbusMapper mapper = new(server);
sp1.Register(mapper);
sp2.Register(mapper);
sp3.Register(mapper);
pc.Register(mapper);
bt.Register(mapper);

var simulationTask = logic.RunSimulation();
server.Listen();
await simulationTask;