using Serilog;
using SpendShelf.BankTransactionsImport.TransactionsParser;
using System;
using Xunit;
using Moq;
using System.IO;

namespace SpendShelf.BankTransactionsImport.Tests.TransactionsParser
{
    public class BankTransactionsXlsParserTests
    {
        [Fact]
        public void Parse_ParsesTransactionsCorrectly_WhenInputValid()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var parser = GetXlsParser();

            var result = parser.ParseTransactions(GetFileStream("Valid3TestTransactions.xls"));

            Assert.Equal(3, result.Count);
            var secondItem = result[1];
            Assert.Equal("Кафе, бари, ресторани", secondItem.Type);
            Assert.Equal("Кафе: КАФЕ", secondItem.Merchant);
            Assert.Equal("грн", secondItem.Currency);
            Assert.Equal(20200, secondItem.Amount);
            Assert.Equal(DateTime.MinValue, secondItem.DateTimeUtc);
        }

        [Fact]
        public void Parse_ThrowException_WhenInputInvalid()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var parser = GetXlsParser();

            Action act = () => parser.ParseTransactions(GetFileStream("Invalid3TestTransactions.xls"));

            Assert.Throws<InvalidCastException>(act);
        }

        private BankTransactionsXlsParser GetXlsParser()
        {
            var dateParserMock = new Mock<IBankTransactionsDateParser>();
            var loggerMock = new Mock<ILogger>();
            dateParserMock.Setup(foo => foo.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns(DateTime.MinValue);
            return new BankTransactionsXlsParser(dateParserMock.Object, loggerMock.Object);
        }

        private Stream GetFileStream(string fileName)
        {
            return File.Open(Path.Combine("TransactionsParser", fileName), FileMode.Open);
        }
    }
}