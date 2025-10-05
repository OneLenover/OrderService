using Refit;

namespace OrderService.API.Clients
{
    // Взаимодействие с платежным сервисом
    public interface IPaymentClient
    {
        [Post("/api/payments/create")]
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request);
    }

    public record CreatePaymentRequest(long OrderId, decimal Price);
    public record CreatePaymentResponse(long Id);
}
