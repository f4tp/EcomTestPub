using EcomTest.Domain.DomainEntities;
using EcomTest.Common.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Domain.DomainServices
{
    public class ProductDomainService : IProductDomainService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductDomainService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProducts(IEnumerable<long>? productIds)
        {
            return await _productRepository.GetAsListAsync(nameof(Product.Id), productIds);
        }

    }
}
