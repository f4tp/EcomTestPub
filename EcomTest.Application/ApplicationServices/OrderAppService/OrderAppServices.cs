using AutoMapper;
using EcomTest.Application.Dtos;
using EcomTest.Common.CustomExceptions;
using EcomTest.Common.DbContexts;
using EcomTest.Domain.DomainEntities;
using EcomTest.Domain.DomainServices;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EcomTest.Application.ApplicationServices
{
    public class OrderAppServices : IOrderAppService
    {
        private readonly IMapper _mapper;
        private readonly IOrderDomainService _orderDomainService;
        private readonly IStockItemDomainService _stockItemDomainService;
        private readonly AppDbContext _dbContext;

        public OrderAppServices(IMapper mapper, IOrderDomainService orderDomainService, IStockItemDomainService stockItemDomainService, AppDbContext dbContext)
        {
            _mapper = mapper;
            _orderDomainService = orderDomainService;
            _stockItemDomainService = stockItemDomainService;
            _dbContext = dbContext;
        }

        public bool CheckOrderTotalPriceEqualsSumOfOrderItems(decimal orderTotalPrice, IEnumerable<OrderItemDtoCreateOrder> allOrderItems)
        {
            decimal orderPriceCalculated = 0;

            foreach (var orderItem in allOrderItems)
            {
                // Ensure that Product is not null before accessing its Price
                if (orderItem.Product != null)
                {
                    orderPriceCalculated += orderItem.Product.BasePrice * orderItem.Quantity;
                }
            }

            if (orderPriceCalculated == orderTotalPrice)
                return true;

            //safest option returned by default
            return false;
        }

      
        public async Task<OrderDtoCreateOrder> CreateOrderAndOptionalCustomer(OrderDtoCreateOrder orderDtoToCreate)
        {
            //FUTURE CONSIDERATION: some responsibility for Customer, OrderItem, and Product is in here with the transaction, it might be better designed if e.g. the transaction is prop drilled through to the respecting app services but it would make for harder to read solution

            var strategy = _dbContext.Database.CreateExecutionStrategy();
            OrderDtoCreateOrder orderToReturn = null;

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //Some properties are going to be stripped from orderDtoToCreate so theat EF doesn't try to update them unnecessarily / persist them twice, so this holds a copy of the original order
                        var deepCopyOrOrderToCreate = JsonSerializer.Deserialize<OrderDtoCreateOrder>(JsonSerializer.Serialize(orderDtoToCreate));

                       //When persisting the Order, we don't want to update / insert the existing Products
                        foreach (var orderItem in orderDtoToCreate.OrderItems)
                        {
                            orderItem.Product = null;
                            orderItem.StockItems = null;
                        }

                        var mappedOrder = _mapper.Map<Order>(orderDtoToCreate);

                        //remove the customer if they already exist as don't need to create them again / don't need to update
                        if (orderDtoToCreate.Customer.Id.HasValue || orderDtoToCreate.CustomerId.HasValue)
                            mappedOrder.Customer = null;
                        
                        //Persist the Order, OrderItems, and optional Customer
                        var createdOrder = await _orderDomainService.CreateOrderAsync(mappedOrder);
                        var createdOrderDto = _mapper.Map<OrderDtoCreateOrder>(createdOrder);

                        //Update StockItems
                        //Reset the StockItem.OrderItemId now the records above have been created
                        var allStockItemsToUpdate = deepCopyOrOrderToCreate.OrderItems.SelectMany(oi => oi.StockItems);
                        var createdOrderItemsAsList = createdOrderDto.OrderItems.ToList();
                        foreach (var stockItem in allStockItemsToUpdate)
                        {
                            // Check if there's an OrderItem with the same ProductId in createdOrderItems
                            var foundOrderItem = createdOrderDto.OrderItems
                                .Where(oi => oi.ProductId == stockItem.ProductId)
                                .FirstOrDefault();

                            // Check if the foundOrderItem is not null and its OrderItemId is not in allStockItems
                            if (foundOrderItem != null)
                                stockItem.OrderItemId = foundOrderItem.Id;
                        }

                        var stockItemsAsEntCollection = _mapper.Map<IEnumerable<StockItem>>(allStockItemsToUpdate);
                        //write the  OrderItem.Ids against the StockItem.OrderItemId to indicate a relationship (and that these StockItems are no longer in stock)
                        await _stockItemDomainService.UpdateMultiple(stockItemsAsEntCollection);

                        //Update the DTO to be sent back to the client
                        foreach (var orderItem in createdOrderDto.OrderItems)
                            orderItem.StockItems = allStockItemsToUpdate.Where(si => si.OrderItemId == orderItem.Id);

                        transaction.Commit();

                        orderToReturn = createdOrderDto;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });

            return orderToReturn;

        }
    }
}
