using EcomTest.Domain.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Domain.DomainServices
{
    public interface ICustomerDomainService
    {
        Task<Customer> GetByIdNoTrackingAsync(long customerId);
        Task<Customer> GetCustomer(string emailAddress);
    }
}
