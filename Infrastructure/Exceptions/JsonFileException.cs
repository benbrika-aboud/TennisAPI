using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class JsonFileException : Exception
    {
        public JsonFileException() { }
        public JsonFileException(string message) : base(message) { }
        public JsonFileException(string message, Exception inner) : base(message, inner) { }
    }
}
