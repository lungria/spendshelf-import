using System;
using System.Globalization;
using TimeZoneConverter;

namespace SpendShelf.BankTransactionsImport.TransactionsParser
{
    /// <summary>
    /// Date converter to work with bank date format. 
    /// </summary>
    public class BankTransactionsDateParser : IBankTransactionsDateParser
    {
        private const string UkraineTimezoneCode = "Europe/Kiev";

        /// <summary>
        /// Parses bank date and time into DateTime UTC object using predefined "Europe/Kiev" timezone.
        /// Supports both Linux and Windows time zone formats.
        /// </summary>
        /// <param name="transactionDate">Transaction date in string format. Example: "01.01.2020".</param>
        /// <param name="transactionTime">Transaction time in string format. Example: "11:20".</param>
        /// <returns>Utc date time.</returns>
        public DateTime Parse(string transactionDate, string transactionTime)
        {
            var transactionLocalDateTime = DateTime.ParseExact(
                              $"{transactionDate} {transactionTime}",
                              "dd.MM.yyyy HH:mm",
                              CultureInfo.InvariantCulture);

            return TimeZoneInfo.ConvertTimeToUtc(transactionLocalDateTime, TZConvert.GetTimeZoneInfo(UkraineTimezoneCode));
        }
    }
}
