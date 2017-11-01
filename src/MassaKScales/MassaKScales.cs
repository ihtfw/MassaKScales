using System;

namespace MassaKScales
{
    using System.IO;
    using System.Reflection;

    using global::MassaKScales.Exceptions;

    using ScalesMassaK;

    public class MassaKScales : IDisposable
    {
        private readonly Scale scale = new ScalesMassaK.Scale();
        private readonly object lockObject = new object();

        public MassaKScales()
        {
            var dllName = "ScalesMassaK.dll";
            var exportPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, dllName);
            if (!File.Exists(exportPath))
            {
                Assembly.GetExecutingAssembly().ExtractResourceToFile(dllName, exportPath);
            }
        }

        public bool IsConnected { get; private set; }
        public void Disconnect()
        {
            lock (lockObject)
            {
                scale.CloseConnection();

                IsConnected = false;
            }
        }

        public void Connect(string comPort)
        {
            lock (lockObject)
            {
                Disconnect();

                scale.Connection = comPort;
                if (scale.OpenConnection() == 0)
                {
                    IsConnected = true;
                    return;
                }
                
                throw new ConnectionException();
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

                var res = scale.ReadWeight();
                if (res != 0)
                {
                    throw new GetInfoException("Weigth");
                }

                switch (scale.Division)
                {
                    case 0: //mg
                        return scale.Weight * 10.0; 
                    case 1: //g
                        return scale.Weight;
                    case 2: //kg
                        return scale.Weight / 1000.0;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(scale.Division), $"scale.Division was {scale.Division}");
                }
            }
        }
        
        public void Dispose()
        {
            Disconnect();
        }
    }
}
