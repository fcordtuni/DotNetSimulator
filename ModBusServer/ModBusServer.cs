/// <summary>
/// Autor: Elisabeth Gisser
/// </summary>

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

                Console.WriteLine($"Modbus Server started. Waiting for connections on Port {port}");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine($"Client connected: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");

                    HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting the server: {ex.Message}");

            }
        }

        /// <summary>
        /// Handles communication with a connected client
        /// </summary>
        /// <param name="client">The TcpClient representing the connected client</param>
        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    byte[] requestBuffer = new byte[256];
                    int bytesRead = stream.Read(requestBuffer, 0, requestBuffer.Length);

                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Client disconnected.");
                        break;
                    }

                    int functionCode = requestBuffer[1];

                    if (functionCode == 0x03)
                    {
                        // Processing read request
                        int startingAddress = BitConverter.ToUInt16(requestBuffer, 2);
                        int quantity = BitConverter.ToUInt16(requestBuffer, 4);

                        byte[] responseBuffer = new byte[3 + quantity * 2];
                        responseBuffer[0] = requestBuffer[0];
                        responseBuffer[1] = requestBuffer[1];
                        responseBuffer[2] = (byte)(quantity * 2);

                        for (int i = 0; i < quantity; i++)
                        {
                            int registerAddress = startingAddress + i;
                            int value = deviceRegisters.GetValueByAddress((ModbusRegistersAddresses)registerAddress);

                            byte[] valueBytes = BitConverter.GetBytes((ushort)value);
                            responseBuffer[3 + i * 2] = valueBytes[0];
                            responseBuffer[4 + i * 2] = valueBytes[1];
                        }

                        stream.Write(responseBuffer, 0, responseBuffer.Length);
                        Console.WriteLine($"Sent Modbus response to client: {BitConverter.ToString(responseBuffer)}");
                    }
                    else if (functionCode == 0x06)
                    {
                        // Processing write single register request
                        int registerAddress = BitConverter.ToUInt16(requestBuffer, 2);
                        int valueToWrite = BitConverter.ToUInt16(requestBuffer, 4);

                        deviceRegisters.SetValueByAddress((ModbusRegistersAddresses)registerAddress, valueToWrite);

                        byte[] responseBuffer = new byte[6];
                        Array.Copy(requestBuffer, responseBuffer, 6);
                        stream.Write(responseBuffer, 0, responseBuffer.Length);
                        Console.WriteLine($"Sent Modbus response to client: {BitConverter.ToString(responseBuffer)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine("Client disconnected.");
            }
        }


        /// <summary>
        /// Stops the Modbus server
        /// </summary>
        public void Stop()
        {
            listener?.Stop();
            Console.WriteLine("Modbus Server stopped.");
        }
    }
}
