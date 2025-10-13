using FluentValidation;

namespace OrderService.API.UseCases.CreateOrder
{
    // Проверка корректности данных в команде
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator() 
        {
            RuleFor(v => v.ProductId).GreaterThan(0);
            RuleFor(v => v.Amount).GreaterThan(0);
            RuleFor(v => v.EmailClient).NotEmpty().EmailAddress().MaximumLength(200);
            RuleFor(v => v.Price).GreaterThan(0);
            RuleFor(v => v.PhoneNumber).NotEmpty().MaximumLength(50);
        }
    }
}
