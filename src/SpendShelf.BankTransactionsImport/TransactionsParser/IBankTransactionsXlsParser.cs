using System.Collections.Generic;
using System.IO;

namespace SpendShelf.BankTransactionsImport.TransactionsParser
{
    public interface IBankTransactionsXlsParser
    {
        List<BankTransactionInfo> ParseTransactions(Stream transactionsFileStream);
    }
}