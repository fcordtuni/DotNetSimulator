using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;

namespace ModBusServer
{
    internal class SimulationModbusServer
    {
        private ModbusServer server;

        private SimulationModbusServer()
        {
            server = new ModbusServer();
        }

        private void doSomething()
        {
            server.Listen();
        }
    }
}
