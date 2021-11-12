using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Infracciones.Helpers
{
    public enum PaymentStatus
    {
        [Description("No pagado")]
        NoPagado,

        [Description("Pagado")]
        Pagado,

        [Description("Cancelado")]
        Cancelado
    }
}
