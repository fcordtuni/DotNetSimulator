using System;
using System.Net;
using System.Net.Sockets;

namespace ModBusServer
{
    /// <summary>
    /// Represents a Modbus server that listens for client connections
    /// </summary>
    public class ModbusServer
    {
        private TcpListener listener;
        private IDeviceModbus deviceRegisters;

        /// <summary>
        /// Constructor for ModbusServer class
        /// </summary>
        /// <param name="device">An instance of a device implementing the IDeviceModbus interface</param>
        public ModbusServer(IDeviceModbus device)
        {
            deviceRegisters = device;
        }

        /// <summary>
        /// Starts the Modbus server on the specified port and waits for client connections
        /// </summary>
        /// <param name="port">The port number to listen for incoming connections</param>
        public void Start(int port)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                Console.WriteLine($"Modbus Server gestartet. Warte auf Verbindungen auf Port {port}");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine($"Client verbunden: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");

                    HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Starten des Servers: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles communication with a connected client
        /// </summary>
        /// <param name="client">The TcpClient representing the connected client</param>
        private void HandleClient(TcpClient client)
        {
            int registerAddress = 0x0001;
            int value = deviceRegisters.GetValueByAddress((ModbusRegistersAddresses)registerAddress);
        }

        /// <summary>
        /// Stops the Modbus server
        /// </summary>
        public void Stop()
        {
            listener?.Stop();
            Console.WriteLine("Modbus Server gestoppt.");
        }
    }
}
