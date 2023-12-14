using EcomTest.Domain.DomainEntities;
using EcomTest.Common.DbContexts;


namespace EcomTest.Domain.DomainServices
{
    public class CustomerDomainService : ICustomerDomainService
    {
        private readonly IRepository<Customer> _customerRepository;
        public CustomerDomainService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> GetByIdNoTrackingAsync(long customerId)
        {
            return await _customerRepository.GetByIdNoTrackingAsync(customerId);
        }

        public async Task<Customer>GetCustomer(string emailAddress)
        {
            return await _customerRepository.GetSingleOrDefaultAsync(nameof(Customer.Email), emailAddress);
        }
    }
}
