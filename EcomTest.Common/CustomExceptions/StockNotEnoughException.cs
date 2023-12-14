using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Common.CustomExceptions
{
    public class StockNotEnoughException : Exception
    {
        public StockNotEnoughException()
        {
        }

        public StockNotEnoughException(string message) : base(message)
        {
        }

        public StockNotEnoughException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
