using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOF.Repositories.EFCore.ExceptionHandling
{
    public class CrudException : Exception
    {
        public CrudException() { }

        public CrudException(string message) : base(message) { }

        public CrudException(string message, Exception innerException) : base(message, innerException) { }
    }
}
