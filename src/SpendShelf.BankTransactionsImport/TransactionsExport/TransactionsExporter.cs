using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Serilog;
using SpendShelf.BankTransactionsImport.Infrastructure;
using SpendShelf.BankTransactionsImport.TransactionsParser;

namespace SpendShelf.BankTransactionsImport.TransactionsExport
{
    public class TransactionsExporter : ITransactionsExporter
    {
        private const string TopicName = "/transactions/privat";

        private readonly IMqttClient _mqttClient;
        private readonly MqttClientConfig _config;
        private readonly ILogger _logger;

        public TransactionsExporter(IMqttClient mqttClient, IOptions<MqttClientConfig> config, ILogger logger)
        {
            _config = config.Value;
            _mqttClient = mqttClient;
            _logger = logger;
        }

        public async Task SendMessage(List<BankTransactionInfo> transactions, CancellationToken cancellationToken)
        {
            await Connect(cancellationToken);
            
            var data = JsonSerializer.SerializeToUtf8Bytes(transactions);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(TopicName)
                .WithAtLeastOnceQoS()
                .WithPayload(data)
                .Build();
            
            _logger.Information($"Publishing message to MqttServer with {TopicName} topic.");
            await _mqttClient.PublishAsync(message);
        }

        private async Task Connect(CancellationToken cancellationToken)
        {
            if (!_mqttClient.IsConnected)
            {
                _logger.Information($"Connecting to MqttServer - {_config.ServerUrl}");
                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(_config.ServerUrl)
                    .Build();

                await _mqttClient.ConnectAsync(options, cancellationToken);
            }
        }
    }
}
