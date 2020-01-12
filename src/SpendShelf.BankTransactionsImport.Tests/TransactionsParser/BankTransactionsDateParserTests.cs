using SpendShelf.BankTransactionsImport.TransactionsParser;
using System;
using System.Collections.Generic;
using Xunit;

namespace SpendShelf.BankTransactionsImport.Tests.TransactionsParser
{
    public class BankTransactionsDateParserTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Parse_When_Valid(string value1, string value2, DateTime expected)
        {
            var bankTransactionsDateParser = new BankTransactionsDateParser();
            new DateTime();
            var result = bankTransactionsDateParser.Parse(value1, value2);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Data =>
    new List<object[]>
    {
            new object[] { "08.01.2020", "12:50", new DateTime(2020, 1, 8, 10, 50, 0, DateTimeKind.Utc)},
            new object[] { "08.01.2020", "00:00", new DateTime(2020, 1, 7, 22, 0, 0, DateTimeKind.Utc)},
            new object[] { "28.01.2020", "09:59", new DateTime(2020, 1, 28, 7, 59, 0, DateTimeKind.Utc)},
            new object[] { "28.11.2020", "09:59", new DateTime(2020, 11, 28, 7, 59, 0, DateTimeKind.Utc)},
            new object[] { "28.07.2020", "09:59", new DateTime(2020, 07, 28, 6, 59, 0, DateTimeKind.Utc)},
            new object[] { "01.03.2020", "00:50", new DateTime(2020, 2, 29, 22, 50, 0, DateTimeKind.Utc)},
            new object[] { "28.11.2020", "09:59", new DateTime(2020, 11, 28, 7, 59, 0, DateTimeKind.Utc)},
    };

    }
}
