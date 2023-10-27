using System;

public class ModBusServer
{
    private ModBusServer modbusServer;

    public ModBusServer(int port)
    {
        modbusServer = new ModBusServer(port);
    }

    public void StartServer()
    {
        try
        {
            Console.WriteLine("Modbus Server gestartet. Warten auf Verbindungen...");
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
            Console.WriteLine("Modbus Server gestoppt.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Stoppen des Modbus-Servers: " + ex.Message);
        }
    }
}
