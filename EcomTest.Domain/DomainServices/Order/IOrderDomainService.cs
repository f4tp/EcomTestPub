using EcomTest.Domain.DomainEntities;

namespace EcomTest.Domain.DomainServices
{
    public interface IOrderDomainService
    {
        Task<Order> CreateOrderAsync(Order orderToCreate);
    }
}
