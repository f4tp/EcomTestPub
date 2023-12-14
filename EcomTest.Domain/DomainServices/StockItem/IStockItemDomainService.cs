using EcomTest.Domain.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Domain.DomainServices
{
    public interface IStockItemDomainService
    {
        Task<IEnumerable<StockItem>> GetStockItemsForProductsNoTracking(IEnumerable<long> productIds);
        Task UpdateMultiple(IEnumerable<StockItem> stockItemsToUpdate);
    }
}
