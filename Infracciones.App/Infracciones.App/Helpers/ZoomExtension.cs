using System;
using System.Collections.Generic;
using System.Text;

namespace Infracciones.Helpers
{
    public static class ZoomExtension
    {
        public static double Clamp(this double self, double min, double max)
        {
            return Math.Min(max, Math.Max(self, min));
        }
    }
}
