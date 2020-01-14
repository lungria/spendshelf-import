using System;

namespace SpendShelf.BankTransactionsImport.TransactionsParser
{
    /// <summary>
    /// Transaction general information.
    /// </summary>
    public class BankTransactionInfo
    {
        /// <summary>
        /// Transaction date time converted to Utc format from Ukrainian time zone.
        /// </summary>
        public DateTime DateTimeUtc { get; set; }

        /// <summary>
        /// Transaction type as is from import file.
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// Merchant (including location) as is from import file.
        /// </summary>
        public string Merchant { get; set; } = default!;

        /// <summary>
        /// Transaction amount as is from import file.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Transaction currency as is from import file.
        /// </summary>
        public string Currency { get; set; } = default!;
    }
}
