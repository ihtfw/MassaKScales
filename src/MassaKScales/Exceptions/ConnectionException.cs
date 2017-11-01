using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassaKScales.Exceptions
{
    public class ConnectionException : Exception
    {
        public ConnectionException() : base("Failed to connect to scales")
        {
        }
    }
}
