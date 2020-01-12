using System;

namespace SpendShelf.BankTransactionsImport.TransactionsParser
{
    public interface IBankTransactionsDateParser
    {
        DateTime Parse(string transactionDate, string transactionTime);
    }
}