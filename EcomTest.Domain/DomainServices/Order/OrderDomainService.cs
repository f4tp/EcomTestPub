using EcomTest.Common.DbContexts;
using EcomTest.Domain.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Domain.DomainServices
{
    public class OrderDomainService : IOrderDomainService
    {
        private readonly IRepository<Order> _orderRepository;
        public OrderDomainService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(Order orderToCreate)
        {
            return await _orderRepository.AddAndReturnCreatedAsync(orderToCreate);
        }
    }
}
