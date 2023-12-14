using EcomTest.Domain.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Domain.DomainServices
{
    public interface IProductDomainService
    {
        Task<IEnumerable<Product>> GetProducts(IEnumerable<long>? productIds);
    }
}
