using System.Globalization;

namespace Shopping_Kart
{
    public static class CurrencyExtensions
    {
        /// <summary>
        /// Check whether input is valid currency
        /// </summary>
        /// <param name="currency">A string potentially containing currency information</param>
        /// <returns>true if valid; otherwise false.</returns>
        public static bool IsValidCurrency(string? currency)
        {
            if (currency == null) return false;
            decimal d;

            if (decimal.TryParse(currency, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-GB"), out d))

                return (d * (decimal)Math.Pow(10, 2) % 1 == 0);
            else return false;
        }
    }
}
