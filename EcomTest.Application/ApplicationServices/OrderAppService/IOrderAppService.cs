using EcomTest.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Application.ApplicationServices
{
    public interface IOrderAppService
    {
        bool CheckOrderTotalPriceEqualsSumOfOrderItems(decimal orderTotalPrice, IEnumerable<OrderItemDtoCreateOrder> allOrderItems);
        Task<OrderDtoCreateOrder> CreateOrderAndOptionalCustomer(OrderDtoCreateOrder orderToCreate);
    }
}
