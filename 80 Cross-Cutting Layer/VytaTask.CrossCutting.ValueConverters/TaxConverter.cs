using System;
using System.Globalization;

namespace VytaTask.CrossCutting.ValueConverters
{
    public static class TaxConverter
    {
        public static string DecimalToQuantityString(decimal amount, IFormatProvider provider = null)
        {
            if (provider == null)
                provider = CultureInfo.CurrentCulture;

            return amount.ToString("C", provider);
        }
    }
}
