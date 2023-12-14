using EcomTest.Common.DbContexts;
using EcomTest.Domain.DomainEntities;


namespace EcomTest.Domain.DomainServices
{
    public class StockItemDomainService : IStockItemDomainService
    {
        private readonly IRepository<StockItem> _stockItemRepository;

        public StockItemDomainService(IRepository<StockItem> stockItemRepository)
        {
            _stockItemRepository = stockItemRepository;
        }
        
        public async Task<IEnumerable<StockItem>> GetStockItemsForProductsNoTracking(IEnumerable<long> productIds)
        {
            return await _stockItemRepository.GetAsListAsync(nameof(StockItem.ProductId), productIds, false, cond => cond.OrderItemId == null);
        }

        public async Task UpdateMultiple(IEnumerable<StockItem> stockItemsToUpdate)
        {
            await Task.Run(() => _stockItemRepository.UpdateMultipleAsync(stockItemsToUpdate));
        }

    }
}
