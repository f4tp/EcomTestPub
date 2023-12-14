using AutoMapper;
using EcomTest.Application.Dtos;
using EcomTest.Domain.DomainServices;

namespace EcomTest.Application.ApplicationServices
{
    public class StockItemAppService : IStockItemAppService
    {
        private readonly IStockItemDomainService _stockItemDomainService;
        private readonly IMapper _mapper;
        public StockItemAppService(IMapper mapper, IStockItemDomainService stockItemDomainService)
        {
            _mapper = mapper;
            _stockItemDomainService = stockItemDomainService;
        }

        public async Task<IEnumerable<StockItemDtoCreateOrder>> GetStockItemsForProducts(IEnumerable<long> productIds)
        {
            return _mapper.Map<IEnumerable<StockItemDtoCreateOrder>>(await _stockItemDomainService.GetStockItemsForProductsNoTracking(productIds));
        }
    }
}
