using System;
using System.Net;
using System.Net.Sockets;

public class ModbusServer
{
    private TcpListener tcpListener;

    public ModbusServer(int port)
    {
        tcpListener = new TcpListener(IPAddress.Any, port);
    }

    public void StartServer()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Modbus Server gestartet. Warten auf Verbindung...");
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Neue Verbindung hergestellt.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Starten des Modbus Servers: " + ex.Message);
        }
    }

    public void StopServer()
    {
        try
        {
            tcpListener.Stop();
            Console.WriteLine("Modbus Server gestoppt.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Stoppen des Modbus Servers: " + ex.Message);
        }
    }
}
