namespace MassaKScales.Exceptions
{
    using System;

    public class GetInfoException : Exception
    {
        public GetInfoException(string message) : base($"Failed to get {message} from scales")
        {
        }
    }
}