using AutoMapper;
using EcomTest.Application.Dtos;
using EcomTest.Domain.DomainServices;

namespace EcomTest.Application.ApplicationServices
{
    public class ProductAppService : IProductAppService
    {

        private readonly IMapper _mapper;
        private readonly IProductDomainService _productDomainService;

        public ProductAppService(IMapper mapper, IProductDomainService productDomainService)
        {
            _mapper = mapper;
            _productDomainService = productDomainService;
        }
        public async Task<IEnumerable<ProductDtoCreateOrder>> GetProducts(IEnumerable<long> productIds)
        {
            return _mapper.Map<IEnumerable<ProductDtoCreateOrder>>(await _productDomainService.GetProducts(productIds));
        }
    }
}
