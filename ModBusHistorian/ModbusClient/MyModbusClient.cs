//Author: FCORDT

using NLog;

namespace ModBusHistorian.ModbusClient
{
    public class MyModbusClient : IMyModbusClient
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        public int PollingTimeMs { get; set; }
        public int ReconnectTimeMs { get; set; }
        public bool IsAutoReconnectEnabled { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void AddReadInputRegisterPolling(AddressRange addressRange)
        {
            throw new NotImplementedException();
        }

        public int? GetInputRegisterValue(int address)
        {
            throw new NotImplementedException();
        }

        public void WriteCoil(int address, bool value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
