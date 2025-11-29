using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Health.Claims.Service.Infrastructure.Messaging
{
    public class ProducerCleanupService<T> : IHostedService
    {
        private readonly T _producer;

        public ProducerCleanupService(T producer)
        {
            _producer = producer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Nothing to start
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
