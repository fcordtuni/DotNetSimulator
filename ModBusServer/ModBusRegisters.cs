using System;
using EasyModbus;

namespace ModBusRegisters
{
    class ModBusRegisters
    {
        // Informationsregister
        public ushort Seriennummer { get; set; }
        public ushort HerstellerID { get; set; }
        public ushort ProduktID { get; set; }
        public ushort ProduktTyp { get; set; }
        public ushort Versionsnummer { get; set; }

        // Kenngrößen
        public int MaxLeistungAusgangWatt { get; set; }
        public float MaxSpannungAusgangVolt { get; set; }
        public float MaxStromAusgangAmpere { get; set; }
        public float Wirkungsgrad { get; set; }
        public float FrequenzausgangHertz { get; set; }

        // Aktuelle Betriebsgrößen
        public int LeistungAusgangWatt { get; set; }
        public float SpannungAusgangVolt { get; set; }
        public float StromAusgangAmpere { get; set; }
        public float FrequenzHertz { get; set; }
        public string Betriebsmodus { get; set; }
        public string Betriebsstatus { get; set; }
        public float TemperaturCelsius { get; set; }
        public string Fehlercodes { get; set; }

        // Betriebsparameter
        public string BetriebsmodusEinstellung { get; set; }
        public int MaximaleLeistungseinstellung { get; set; }
        public int Spannungseinstellung { get; set; }
        public int Frequenzeinstellung { get; set; }
        public bool SchutzfunktionenAktiviert { get; set; }
        public int Einschaltverzögerung { get; set; }
        public string Kommunikationseinstellungen { get; set; }
        public int UpdateIntervalle { get; set; }
        public bool EnergiesparmodusAktiviert { get; set; }

        // Funktionsregister
        public bool StartFunktion { get; set; }
        public bool StopFunktion { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ModBusRegisters modBusRegisters = new ModBusRegisters();

            try
            {
                string ipAddress = "IP-Adresse des Modbus-Geräts";
                int port = 502;

                ModbusClient modbusClient = new ModbusClient(ipAddress, port);

                modbusClient.Connect();

                int neueMaximaleLeistung = 5000;
                modbusClient.WriteSingleRegister(1008, (ushort)neueMaximaleLeistung);

                modBusRegisters.LeistungAusgangWatt = modbusClient.ReadInputRegisters(1000, 1)[0];

                modbusClient.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }
    }
}
