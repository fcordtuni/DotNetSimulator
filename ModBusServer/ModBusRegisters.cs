using EasyModbus;
using System;
using System.Collections.Generic;

namespace ModBus
{
    public class ModBusRegister
    {
        private ModbusClient modbusClient;
        private Dictionary<string, int> registers;

        public ModBusRegister(string ipAddress, int modbusPort)
        {
            modbusClient = new ModbusClient(ipAddress, modbusPort);

            registers = new Dictionary<string, int>
            {
                // Informationsregister
                { "Seriennummer", 12345 },
                { "HerstellerID", 567 },
                { "ProduktID", 890 },
                { "ProduktTyp", 1 },
                { "Versionsnummer", 1 },

                // Kenngrößen
                { "MaxLeistung", 5000 },
                { "MaxSpannung", 230 },
                { "MaxStrom", 20 },
                { "Wirkungsgrad", 90 },
                { "Frequenzausgang", 50 },

                // Aktuelle Betriebsgrößen
                { "Leistungsausgang", 4500 },
                { "Spannungsausgang", 220 },
                { "Stromausgang", 18 },
                { "Frequenz", 50 },
                { "Betriebsmodus", 0 },
                { "Betriebsstatus", 1 },
                { "Temperatur", 25 },
                { "Fehlercodes", 0 },

                // Betriebsparameter
                { "Betriebsmoduseinstellung", 0 },
                { "MaximaleLeistungseinstellung", 5000 },
                { "Spannungseinstellung", 230 },
                { "Frequenzeinstellung", 50 },
                { "Schutzfunktionseinstellungen", 0 },
                { "Einschaltverzögerung", 5 },
                { "Kommunikationseinstellungen", 1 },
                { "UpdateIntervalle", 10 },
                { "Energiesparmodus", 0 },

                // Funktionsregister
                { "StartStopFunktion", 0 }
            };

        }

        public int ReadRegister(int registerAddress)
        {
            int value = -1;

            try
            {
                int[] registerValue = modbusClient.ReadHoldingRegisters(registerAddress, 1);
                value = registerValue[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Lesen des Registers: " + ex.Message);
            }

            return value;
        }

        public void WriteRegister(int registerAddress, int value)
        {
            try
            {
                modbusClient.WriteSingleRegister(registerAddress, (ushort)value);
                Console.WriteLine("Register " + registerAddress + " auf Wert " + value + " gesetzt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Schreiben des Registers: " + ex.Message);
            }
        }
    }
}