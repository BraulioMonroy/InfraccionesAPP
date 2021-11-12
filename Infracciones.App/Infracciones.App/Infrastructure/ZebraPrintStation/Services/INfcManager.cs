using System;

namespace Infracciones.Infrastructure.ZebraPrintStation.Services
{
    public interface INfcManager
    {
        event EventHandler<string> TagScanned;
        bool IsNfcAvailable();
    }
}
