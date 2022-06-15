using MarquitoUtils.Main.Class.Tools;

namespace MarquitoUtils.Main.Class.Entities.Currency
{
    /// <summary>
    /// Currency contain value and symbol
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// The ISO currency symbol, like EUR for Euro
        /// </summary>
        public string ISOcurrencySymbol { get; private set; }

        /// <summary>
        /// The currency symbole found
        /// </summary>
        public string CurrencySymbol { get; private set; }

        /// <summary>
        /// The value of this currency
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// A currency, with value and it's symbol
        /// </summary>
        /// <param name="value">The value of this currency</param>
        /// <param name="ISOcurrencySymbol">The ISO currency symbol, like EUR for Euro</param>
        public Currency(double value, string ISOcurrencySymbol)
        {
            this.Value = value;
            this.ISOcurrencySymbol = ISOcurrencySymbol;
            this.CurrencySymbol = CurrencyTool.getCurrencySymbol(ISOcurrencySymbol);
        }
    }
}
