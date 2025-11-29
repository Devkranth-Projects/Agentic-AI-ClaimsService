using Health.Claims.Service.Application.Interfaces;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Threading;

public class RabbitMqProducer : IMessageProducer
{
    private readonly IConfiguration _configuration;
    private readonly string _hostname;

    public RabbitMqProducer(IConfiguration configuration)
    {
        _configuration = configuration;
        // Read the hostname from configuration (e.g., appsettings.json)
        _hostname = _configuration["RabbitMQ:HostName"]
                    ?? throw new ArgumentNullException("RabbitMQ:HostName configuration is missing.");
    }

    public async Task PublishMessage<T>(T message)
    {
        // 1. Setup Connection Factory
        var factory = new ConnectionFactory { HostName = _hostname };

        // 2. Create Connection and Channel Asynchronously
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        // 3. Declare Queue Asynchronously
        string queueName = "claims_submitted";
        await channel.QueueDeclareAsync(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

        // 4. Serialize Message
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        // 5. Publish Asynchronously
        await channel.BasicPublishAsync<BasicProperties>(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            // *** FINAL FIX: Use the null-forgiving operator (!) on default. ***
            // This tells the compiler that the value can be null, resolving the NRT error.
            basicProperties: default!,
            body: body,
            cancellationToken: CancellationToken.None
        );
    }
}