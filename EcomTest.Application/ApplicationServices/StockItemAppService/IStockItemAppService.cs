using EcomTest.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Application.ApplicationServices
{
    public interface IStockItemAppService
    {
        Task<IEnumerable<StockItemDtoCreateOrder>> GetStockItemsForProducts(IEnumerable<long> productIds);
    }
}
