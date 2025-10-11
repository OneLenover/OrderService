using AutoMapper;
using OrderService.API.DTOs;
using OrderService.API.UseCases.CreateOrder;
using OrderService.API.UseCases.GetOrder;
using OrderService.DataAccess.Postgres.Entities;

namespace OrderService.API.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile() 
        {
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderCommand, Order>();
            CreateMap<Order, OrderCreatedEvent>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(_ => "OrderCreated"));
        }
    }
}
