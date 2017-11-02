using System;

namespace MassaKScales
{
    using System.IO.Ports;

    using global::MassaKScales.Exceptions;

    public class MassaKScales : IDisposable
    {
        private SerialPort serialPort;
        private readonly object lockObject = new object();
        
        public bool IsConnected { get; private set; }
        public void Disconnect()
        {
            lock (lockObject)
            {
                serialPort?.Dispose();
                serialPort = null;
                IsConnected = false;
            }
        }

        public void Connect(string comPort)
        {
            lock (lockObject)
            {
                Disconnect();

                try
                {
                    serialPort = new SerialPort(comPort)
                                 {
                                     BaudRate = 4800,
                                     Parity = Parity.Even,
                                     DataBits = 8,
                                     StopBits = StopBits.One,
                                     ReadTimeout = 3000
                                 };
                    serialPort.Open();

                    serialPort.Write(new byte[] { 0x44 }, 0, 1);
                    var response = ReadResponse();
                    if (response == null)
                        throw new ConnectionException();

                    IsConnected = true;
                }
                catch (Exception e)
                {
                    throw new ConnectionException(e);
                }
            }
        }
        
        

        /// <summary>
        /// In gramms
        /// </summary>
        /// <returns></returns>
        public double GetWeight()
        {
            lock (lockObject)
            {
                if (!IsConnected)
                {
                    throw new ConnectionException();
                }

                serialPort.Write(new byte[] { 0x45 }, 0, 1);

                var response = ReadResponse();

                var w = response[1] * 256 + response[0];

                return w;
            }
        }

        private byte[] ReadResponse()
        {
            var length = 2;
            var response = new byte[length];
            var offset = 0;
            while (offset < length)
            {
                var b = serialPort.ReadByte();
                response[offset] = (byte)b;
                offset++;
            }
            return response;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
