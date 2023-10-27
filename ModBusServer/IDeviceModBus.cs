namespace ModBusServer
{
    public interface IDeviceModbus
    {
        int Seriennummer { get; }
        int HerstellerID { get; }
        int ProduktID { get; }
        int ProduktTyp { get; }
        int Versionsnummer { get; }
        int MaxLeistung { get; }
        int MaxSpannung { get; }
        int MaxStrom { get; }
        int Wirkungsgrad { get; }
        int Frequenzausgang { get; }
        int Leistungsausgang { get; }
        int Spannungsausgang { get; }
        int Stromausgang { get; }
        int Frequenz { get; }
        int Betriebsmodus { get; }
        int Betriebsstatus { get; }
        int Temperatur { get; }
        int Fehlercodes { get; }
        int Betriebsmoduseinstellung { get; }
        int MaximaleLeistungseinstellung { get; }
        int Spannungseinstellung { get; }
        int Frequenzeinstellung { get; }
        int Schutzfunktionseinstellungen { get; }
        int Einschaltverzögerung { get; }
        int Kommunikationseinstellungen { get; }
        int UpdateIntervalle { get; }
        int Energiesparmodus { get; }
        int StartStopFunktion { get; }
    }
}