using AutoMapper;
using EcomTest.Application.Dtos;
using EcomTest.Domain.DomainServices;

namespace EcomTest.Application.ApplicationServices
{
    public class CustomerAppService : ICustomerAppService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerDomainService _customerDomainService;
        public CustomerAppService(IMapper mapper, ICustomerDomainService customerDomainService)
        {
            _mapper = mapper;
            _customerDomainService = customerDomainService;
        }

        public async Task<CustomerDtoCreateOrder> GetCustomerNoTracking(long customerid)
        {
            return _mapper.Map<CustomerDtoCreateOrder>(await _customerDomainService.GetByIdNoTrackingAsync(customerid));
        }

        public async Task<CustomerDtoCreateOrder> GetCustomer(string emailAddress)
        {
            return _mapper.Map<CustomerDtoCreateOrder>(await _customerDomainService.GetCustomer(emailAddress));
        }
    }
}
