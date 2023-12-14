using EcomTest.Application.Dtos;

namespace EcomTest.Application.ApplicationServices
{
    public interface IProductAppService
    {
        Task<IEnumerable<ProductDtoCreateOrder>> GetProducts(IEnumerable<long> productIds);
    }
}
