using Microsoft.Extensions.Options;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using Serilog;
using SpendShelf.BankTransactionsImport.Infrastructure;
using SpendShelf.BankTransactionsImport.TransactionsExport;
using SpendShelf.BankTransactionsImport.TransactionsParser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using Xunit;

namespace SpendShelf.BankTransactionsImport.Tests.TransactionsExport
{
    public class TransactionsExporterTests
    {
        private Mock<IMqttClient>? mqttClientMock;

        [Fact]
        public void SendMessage_PostToMqttServer_WhenInputValid()
        {
            var exporter = GetTransactionsExporter();

            var transactions = new List<BankTransactionInfo>() {
                new BankTransactionInfo {Amount = 10, Currency = "грн",
                    DateTimeUtc = new DateTime(2020, 11, 28, 7, 59, 0, DateTimeKind.Utc),
                    Merchant = "Test", Type  = "test2"} };
            var result = exporter.SendMessage(transactions, CancellationToken.None);

            var data = JsonSerializer.Serialize(transactions);
            mqttClientMock?.Verify(
                foo => foo.PublishAsync(It.IsAny<MqttApplicationMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private TransactionsExporter GetTransactionsExporter()
        {
            mqttClientMock = new Mock<IMqttClient>();

            var loggerMock = new Mock<ILogger>();
            var optionsMock = new Mock<IOptions<MqttClientConfig>>();
            mqttClientMock.Setup(foo => foo.IsConnected).Returns(false);
            optionsMock.Setup(foo => foo.Value).Returns(new MqttClientConfig { ServerUrl = "localhost" });
            return new TransactionsExporter(mqttClientMock.Object, optionsMock.Object, loggerMock.Object);
        }
    }
}
