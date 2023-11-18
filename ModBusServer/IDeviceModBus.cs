/// <summary>
/// Autor: Elisabeth Gisser
/// </summary>
namespace ModBusServer
{
    /// <summary>
    /// Enum representing Modbus register addresses
    /// </summary>
    public enum ModbusRegistersAddresses
    {
        Seriennummer,
        HerstellerID,
        ProduktID,
        ProduktTyp,
        Versionsnummer,
        MaxLeistung,
        MaxSpannung,
        MaxStrom,
        Wirkungsgrad,
        Frequenzausgang,
        Leistungsausgang,
        Spannungsausgang,
        Stromausgang,
        Frequenz,
        Betriebsmodus,
        Betriebsstatus,
        Temperatur,
        Fehlercodes,
        Betriebsmoduseinstellung,
        MaximaleLeistungseinstellung,
        Spannungseinstellung,
        Frequenzeinstellung,
        Schutzfunktionseinstellungen,
        Einschaltverzögerung,
        Kommunikationseinstellungen,
        UpdateIntervalle,
        Energiesparmodus,
        StartStopFunktion
    }

    /// <summary>
    /// Interface representing a Modbus device
    /// </summary>
    public interface IDeviceModbus
    {
        /// <summary>
        /// Retrieves the value associated with a Modbus register address
        /// </summary>
        /// <param name="address">The Modbus register address</param>
        /// <returns>The value stored at the specified address</returns>
        int GetValueByAddress(ModbusRegistersAddresses address);

        /// <summary>
        /// Sets the value for a specific Modbus register address
        /// </summary>
        /// <param name="address">The Modbus register address</param>
        /// <param name="value">The value to be set</param>
        void SetValueByAddress(ModbusRegistersAddresses address, int value);
    }
}
