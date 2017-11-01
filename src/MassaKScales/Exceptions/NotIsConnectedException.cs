namespace MassaKScales.Exceptions
{
    using System;

    public class NotIsConnectedException : Exception
    {
        public NotIsConnectedException() : base("Not is connected to scales")
        {
        }
    }
}