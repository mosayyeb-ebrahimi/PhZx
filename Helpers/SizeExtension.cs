using System;

namespace PhZx
{
    public static class SizeExtension
    {
        public static string ToSize(this long value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (long)unit)).ToString("0.00 ") + unit.ToString();
        }
    }
}
