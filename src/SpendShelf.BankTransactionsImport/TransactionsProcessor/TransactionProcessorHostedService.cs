using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SpendShelf.BankTransactionsImport.TransactionsExport;
using SpendShelf.BankTransactionsImport.TransactionsParser;

namespace SpendShelf.BankTransactionsImport.TransactionsProcessor
{
    public class TransactionProcessorHostedService : BackgroundService
    {
        private readonly IBankTransactionsXlsParser _bankTransactionsXlsParser;
        private readonly ITransactionsExporter _transactionsExporter;
        private readonly ILogger _logger;
        private readonly ChannelReader<Stream> _channel;

        public TransactionProcessorHostedService(
            IBankTransactionsXlsParser bankTransactionsXlsParser,
            ITransactionsExporter transactionsExporter,
            ILogger logger,
            ChannelReader<Stream> channel)
        {
            _transactionsExporter = transactionsExporter;
            _bankTransactionsXlsParser = bankTransactionsXlsParser;
            _logger = logger;
            _channel = channel;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await foreach (var item in _channel.ReadAllAsync(cancellationToken))
            {
                try
                {
                    var transactions = _bankTransactionsXlsParser.ParseTransactions(item);
                    await _transactionsExporter.SendMessage(transactions, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "An unhandled exception occured in ParseTransactions");
                }
            }
        }
    }
}
