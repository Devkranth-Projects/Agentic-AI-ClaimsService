using Azure.Messaging.ServiceBus;
using Health.Claims.Service.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Health.Claims.Service.Infrastructure.Messaging
{
    public class AzureServiceBusProducer : IMessageProducer
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly string _topicName;

        public AzureServiceBusProducer(IConfiguration configuration)
        {
            _topicName = configuration["AzureServiceBus:TopicName"]
                         ?? throw new ArgumentNullException("AzureServiceBus:TopicName configuration is missing.");

            string connectionString = configuration["AzureServiceBus:ConnectionString"]
                                      ?? throw new ArgumentNullException("AzureServiceBus:ConnectionString configuration is missing.");

            // 1. Create a ServiceBusClient, which manages the connection
            _client = new ServiceBusClient(connectionString);

            // 2. Create a ServiceBusSender for the specific Topic
            _sender = _client.CreateSender(_topicName);
        }

        public async Task PublishMessage<T>(T message)
        {
            // 1. Serialize the message payload to JSON bytes
            string jsonPayload = JsonSerializer.Serialize(message);

            // 2. Create the Azure Service Bus Message
            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonPayload))
            {
                // Optional: Set a subject for message filtering (useful for subscriptions with filters)
                Subject = typeof(T).Name
            };

            // 3. Send the message asynchronously to the Topic
            try
            {
                await _sender.SendMessageAsync(serviceBusMessage);

                // Note: Unlike RabbitMQ where we disposed the channel/connection on every call,
                // the ServiceBusClient and ServiceBusSender are designed to be long-lived 
                // and managed by Dependency Injection.
            }
            catch (Exception ex)
            {
                // Handle transient faults or logging here
                Console.WriteLine($"Error publishing to Azure Service Bus: {ex.Message}");
                throw;
            }
        }

        // Recommended: Implement IAsyncDisposable to ensure clean shutdown of the client.
        public async ValueTask DisposeAsync()
        {
            // Dispose of the sender and client when the application shuts down.
            await _sender.DisposeAsync();
            await _client.DisposeAsync();
        }
    }
}
