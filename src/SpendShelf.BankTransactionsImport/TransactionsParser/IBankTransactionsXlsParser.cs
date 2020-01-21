using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SpendShelf.BankTransactionsImport.TransactionsParser
{
    public interface IBankTransactionsXlsParser
    {
        List<BankTransactionInfo> ParseTransactions(Stream transactionsFile);
    }
}