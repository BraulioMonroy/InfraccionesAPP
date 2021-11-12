using System;

namespace Infracciones.Infrastructure.ZebraPrintStation.Exceptions
{
    public class PrinterNotReadyException : Exception
    {
        public PrinterNotReadyException(string message) : base(message) { }
    }
}
