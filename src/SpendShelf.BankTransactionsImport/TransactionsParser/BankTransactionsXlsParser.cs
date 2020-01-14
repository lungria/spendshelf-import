using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExcelDataReader;
using Serilog;

namespace SpendShelf.BankTransactionsImport.TransactionsParser
{
    /// <summary>
    /// Xls converter to work with bank transactions file. 
    /// </summary>
    public class BankTransactionsXlsParser : IBankTransactionsXlsParser
    {
        private readonly IBankTransactionsDateParser _bankTransactionsDateParser;

        private readonly ILogger _logger;

        public BankTransactionsXlsParser(IBankTransactionsDateParser bankTransactionsDateParser, ILogger logger)
        {
            _logger = logger;
            _bankTransactionsDateParser = bankTransactionsDateParser;
        }

        /// <summary>
        /// Parses file steam with bank transaction into object collection.
        /// </summary>
        /// <param name="transactionsFileStream">Stream from xls file with transactions.</param>
        /// <returns>Transactions list.</returns>
        public List<BankTransactionInfo> ParseTransactions(Stream transactionsFileStream)
        {
            using var reader = ExcelReaderFactory.CreateReader(transactionsFileStream);

            _logger.Information("Transaction parsing started");

            var result = reader.AsDataSet().Tables[0].Rows.OfType<DataRow>().Skip(2).Select((item, index) =>
            {
                try
                {
                    return new BankTransactionInfo
                    {
                        DateTimeUtc = _bankTransactionsDateParser.Parse((string)item[0], (string)item[1]),
                        Type = (string)item[2],
                        Merchant = (string)item[4],
                        Amount = Convert.ToDecimal(item[7], null),
                        Currency = (string)item[8],
                    };
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"BankTransactionsXlsParser.ParseTransactions. Failed to parse transaction in {index + 1} row");
                    throw;
                }
            }).ToList();

            _logger.Information($"Transaction parsing ended. {result.Count} items processed");
            return result;
        }
    }
}
