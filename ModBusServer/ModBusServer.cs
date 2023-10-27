using System;
using EasyModbus;

namespace ModbusNamespace
{
    public class ModbusServer
    {
        private ModbusServer modbusServer;

        public ModbusServer(int port)
        {
            modbusServer = new ModbusServer(port);
        }

        public void StartServer()
        {
            try
            {
                Console.WriteLine("Modbus Server gestartet. Warten auf Verbindungen...");
                modbusServer.Listen();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Starten des Modbus-Servers: " + ex.Message);
            }
        }

        public void StopServer()
        {
            try
            {
                modbusServer.StopListening();
                Console.WriteLine("Modbus Server gestoppt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Stoppen des Modbus-Servers: " + ex.Message);
            }
        }
    }
}
