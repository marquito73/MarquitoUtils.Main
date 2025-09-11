using System.Globalization;

namespace MarquitoUtils.Main.Class.Tools
{
    /// <summary>
    /// A currency tool
    /// </summary>
    public class CurrencyUtils
    {
        /// <summary>
        /// Get currency symbol from ISO currency symbol name
        /// </summary>
        /// <param name="ISOCurrencySymbol">ISO currency symbol name</param>
        /// <returns>Currency symbol</returns>
        public static string getCurrencySymbol(string ISOCurrencySymbol)
        {
            string symbol = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
                .Select(ri => ri.CurrencySymbol)
                .FirstOrDefault();
            return symbol;
        }

        public static double GetBaseSizedAmount(double amountToConvert, double baseIncrement)
        {
            // TODO Change double to decimal for resolve precision issues
            return Math.Floor(amountToConvert / baseIncrement) * baseIncrement;
        }
    }
}
