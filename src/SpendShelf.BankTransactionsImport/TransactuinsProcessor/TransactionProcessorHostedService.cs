using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SpendShelf.BankTransactionsImport.TransactionsParser;

namespace SpendShelf.BankTransactionsImport.TransactuinsProcessor
{
    public class TransactionProcessorHostedService : BackgroundService
    {
        private readonly IBankTransactionsXlsParser _bankTransactionsXlsParser;
        private readonly ILogger _logger;
        private readonly ChannelReader<Stream> _channel;

        public TransactionProcessorHostedService(
            IBankTransactionsXlsParser bankTransactionsXlsParser,
            ILogger logger,
            ChannelReader<Stream> channel)
        {
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
                    _bankTransactionsXlsParser.ParseTransactions(item);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "An unhandled exception occured in ParseTransactions");
                }
            }
        }
    }
}
