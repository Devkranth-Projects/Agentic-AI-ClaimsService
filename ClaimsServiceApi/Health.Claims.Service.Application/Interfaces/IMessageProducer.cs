using System.Threading.Tasks;

namespace Health.Claims.Service.Application.Interfaces
{
    public interface IMessageProducer
    {
        // T is a generic type representing the message DTO (e.g., ClaimSubmittedMessage)
        Task PublishMessage<T>(T message);
    }
}