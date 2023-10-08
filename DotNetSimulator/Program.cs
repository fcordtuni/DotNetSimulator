// See https://aka.ms/new-console-template for more information
using DotNetSimulator.Simulator;

SimulationLogic logic = new();
logic.Simulate(DateTime.Now, DateTime.Now.AddDays(1), TimeSpan.FromSeconds(1));
