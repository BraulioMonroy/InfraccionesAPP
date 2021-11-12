using System.Text;

namespace Infracciones.Infrastructure.ZebraPrintStation.Helpers
{
    public static class GetZplFormat
    {
        public static string SanctionReceipt()
        {
            var sb = new StringBuilder();
            sb.Append("^XA");
            sb.Append("^DFE:SANCTIONRECEIPT.ZPL^FS");
            // Logo sedema + title
            sb.Append("^FO108,486^GFA,569,1888,32,:Z64:eJyllM1uwyAMx01kpB5Biu9RnoRI5A4Sfp8c99izaTctqZNtrYtESuP+/PcHAEdLTyf/Msfv+UN+M45y30J8ydsXYCWjT2YAwwTzdObsAIgdawSRqh3A1tepcXPclL+yyZ/nS75j9lUq4IhrsV653WC7XfAb+kX4yOvaTD4MQ+cPJh/Ik6RQ5BPHcMLYLvg+NZ80jjWHZPw+wLQnhxCgr9BxVBvJFqt8zAQc6QkD+uYblZ5wtxbsm9hXJ+6Ad/Vy9jglit4RkYs1OSBHdREBkSvZBTwGMJaSQWKAoq+HRGtCeZLus90f6f/hHyu1MFIhyX9MJA9F9ccT/qH6mKgBjghBgQhIXXZaMWfDWwPYffVCVP0xRNEPbuGefxK+6b4dD9KI2LCNpaX+d410zzmZA/hUf8mZru/6Lypf+68Wq/7S/tfz51Nv/OwzWvpnoU9nzjp/tOj8iXqOVvxi2yWf+/y7vNbF8p9/uX/4fv9EPqn/7ePYAHuTZlA/x9mzpV+6z5r8b76L/eKNalecc7O77s/mdHx7O4Sz++fazGvvZfsEapRCSA==:925C");
            sb.Append("^FT325,456^A0I,23,23^FH\\^CI28^FDMULTAS DE TRÁNSITO^FS");
            // Fields 
            sb.Append("^FT422,415^A0I,20,20^FH\\^FDMOTIVO DE SANCIÓN:^FS");
            sb.Append("^FT422,359^A0I,20,20^FH\\^CI28^FDTIPO:^FS");
            sb.Append("^FT422,334^A0I,20,20^FH\\^CI28^FDPLACA:^FS");
            sb.Append("^FT422,309^A0I,20,20^FH\\^CI28^FDMARCA:^FS");
            sb.Append("^FT422,284^A0I,20,20^FH\\^CI28^FDSUBMARCA:^FS");
            sb.Append("^FT422,259^A0I,20,20^FH\\^CI28^FDMODELO:^FS");
            sb.Append("^FT422,234^A0I,20,20^FH\\^CI28^FDORIGEN:^FS");
            sb.Append("^FT422,209^A0I,20,20^FH\\^CI28^FDFOLIO:^FS");
            sb.Append("^FT422,174^A0I,23,23^FH\\^CI28^FDTOTAL A PAGAR:^FS");
            sb.Append("^FT308,116^A0I,17,18^FH\\^CI28^FDREFERENCIA DE PAGO^FS");
            // Variables   
            sb.Append("^FT422,387^A0I,20,20^FH\\^FN1\"SanctionReason\"^FS");
            sb.Append("^FT305,359^A0I,20,20^FH\\^FN2\"VehicleType\"^FS");
            sb.Append("^FT305,334^A0I,20,20^FH\\^FN3\"Plates\"^FS");
            sb.Append("^FT305,309^A0I,20,20^FH\\^FN4\"Brand\"^FS");
            sb.Append("^FT305,284^A0I,20,20^FH\\^FN5\"SubBrand\"^FS");
            sb.Append("^FT305,259^A0I,20,20^FH\\^FN6\"Model\"^FS");
            sb.Append("^FT305,234^A0I,20,20^FH\\^FN7\"Origin\"^FS");
            sb.Append("^FT305,209^A0I,20,20^FH\\^FN8\"Key\"^FS");
            sb.Append("^FT254,174^A0I,23,23^FH\\^FN9\"Amount\"^FS");
            sb.Append("^FT182,120^BQN,2,4");
            // QR
            sb.Append("^FH\\^FDQRText^FS2");
            sb.Append("^FT334,139^A0I,23,23^FH\\^FN10\"PaymentReference\"^FS");
            // sb.Append("^PQ1,,,Y");
            sb.Append("^XZ");
            return sb.ToString();
        }

        public static string PaymentReceipt()
        {
            var sb = new StringBuilder();
            sb.Append("^XA");
            sb.Append("^DFE:PAYMENTRECEIPT.ZPL^FS");
            // Logo sedema + title
            sb.Append("^FO108,486^GFA,569,1888,32,:Z64:eJyllM1uwyAMx01kpB5Biu9RnoRI5A4Sfp8c99izaTctqZNtrYtESuP+/PcHAEdLTyf/Msfv+UN+M45y30J8ydsXYCWjT2YAwwTzdObsAIgdawSRqh3A1tepcXPclL+yyZ/nS75j9lUq4IhrsV653WC7XfAb+kX4yOvaTD4MQ+cPJh/Ik6RQ5BPHcMLYLvg+NZ80jjWHZPw+wLQnhxCgr9BxVBvJFqt8zAQc6QkD+uYblZ5wtxbsm9hXJ+6Ad/Vy9jglit4RkYs1OSBHdREBkSvZBTwGMJaSQWKAoq+HRGtCeZLus90f6f/hHyu1MFIhyX9MJA9F9ccT/qH6mKgBjghBgQhIXXZaMWfDWwPYffVCVP0xRNEPbuGefxK+6b4dD9KI2LCNpaX+d410zzmZA/hUf8mZru/6Lypf+68Wq/7S/tfz51Nv/OwzWvpnoU9nzjp/tOj8iXqOVvxi2yWf+/y7vNbF8p9/uX/4fv9EPqn/7ePYAHuTZlA/x9mzpV+6z5r8b76L/eKNalecc7O77s/mdHx7O4Sz++fazGvvZfsEapRCSA==:925C");
            sb.Append("^FT325,456^A0I,23,23^FH\\^CI28^FDCOMPROBANTE DE PAGO^FS");
            // Fields 
            sb.Append("^FT422,415^A0I,20,20^FH\\^FDMOTIVO DE SANCIÓN:^FS");
            sb.Append("^FT422,359^A0I,20,20^FH\\^CI28^FDTIPO:^FS");
            sb.Append("^FT422,334^A0I,20,20^FH\\^CI28^FDPLACA:^FS");
            sb.Append("^FT422,309^A0I,20,20^FH\\^CI28^FDMARCA:^FS");
            sb.Append("^FT422,284^A0I,20,20^FH\\^CI28^FDSUBMARCA:^FS");
            sb.Append("^FT422,259^A0I,20,20^FH\\^CI28^FDMODELO:^FS");
            sb.Append("^FT422,234^A0I,20,20^FH\\^CI28^FDORIGEN:^FS");
            sb.Append("^FT422,209^A0I,20,20^FH\\^CI28^FDFOLIO:^FS");
            sb.Append("^FT422,174^A0I,23,23^FH\\^CI28^FDTOTAL A PAGAR:^FS");
            sb.Append("^FT308,116^A0I,17,18^FH\\^CI28^FDREFERENCIA DE PAGO^FS");
            // PAGADO legend
            sb.Append("^FT406,261^A0I,101,101^FH\\^CI28^FDPAGADO^FS");
            // Variables   
            sb.Append("^FT422,387^A0I,20,20^FH\\^FN1\"SanctionReason\"^FS");
            sb.Append("^FT305,359^A0I,20,20^FH\\^FN2\"VehicleType\"^FS");
            sb.Append("^FT305,334^A0I,20,20^FH\\^FN3\"Plates\"^FS");
            sb.Append("^FT305,309^A0I,20,20^FH\\^FN4\"Brand\"^FS");
            sb.Append("^FT305,284^A0I,20,20^FH\\^FN5\"SubBrand\"^FS");
            sb.Append("^FT305,259^A0I,20,20^FH\\^FN6\"Model\"^FS");
            sb.Append("^FT305,234^A0I,20,20^FH\\^FN7\"Origin\"^FS");
            sb.Append("^FT305,209^A0I,20,20^FH\\^FN8\"Key\"^FS");
            sb.Append("^FT254,174^A0I,23,23^FH\\^FN9\"Amount\"^FS");
            sb.Append("^FT182,120^BQN,2,4");
            // QR
            sb.Append("^FH\\^FDQRText^FS2");
            sb.Append("^FT334,139^A0I,23,23^FH\\^FN10\"PaymentReference\"^FS");
            sb.Append("^XZ");
            return sb.ToString();
        }
    }
}
