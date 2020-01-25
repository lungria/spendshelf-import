using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpendShelf.BankTransactionsImport.TransactionsParser;

namespace SpendShelf.BankTransactionsImport.TransactionsExport
{
    public interface ITransactionsExporter
    {
        Task SendMessage(List<BankTransactionInfo> transactions, CancellationToken cancellationToken);
    }
}