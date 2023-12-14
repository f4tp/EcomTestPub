using EcomTest.Application.ApplicationServices;
using EcomTest.Application.Dtos;
using EcomTest.Common.CustomExceptions;
using EcomTest.Common.Helpers;
using EcomTest.Common.MagicStrings.Consts;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace EcomTest.Web.Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CreateOrderController : ControllerBase
    {
        private readonly ILogger<CreateOrderController> _logger;
        private readonly ICustomerAppService _customerAppService;

        private readonly IOrderAppService _orderAppService;
        private readonly IProductAppService _productAppService;
        private readonly IStockItemAppService _stockItemAppService;


        public CreateOrderController(ILogger<CreateOrderController> logger, ICustomerAppService customerAppService, IOrderAppService orderAppService, IProductAppService productAppService, IStockItemAppService stockItemAppService)
        {
            _logger = logger;
            _customerAppService = customerAppService;
            _orderAppService = orderAppService;
            _productAppService = productAppService;
            _stockItemAppService = stockItemAppService;
        }

      
        /// <summary>
        /// Creates an order for the passed in customer, optionally creates a customer if they don't exist yet
        /// </summary>
        /// <param name="orderDtoToCreate"></param>
        /// <returns>Created Order</returns>
        /// <exception cref="CustomerNotFoundException"></exception>
        [HttpPost("CreateCustomerOrder")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateCustomerOrder(OrderDtoCreateOrder orderDtoToCreate)
        {
            if (orderDtoToCreate == null)
                return BadRequest("An order is required.");


            #region Validate Customer
            try
            {

                //don't need to validate if e.g. email has been provided, DTO has been setup to take care of this andwill reject teh request if the model is not valid

                //validate client intention for the customer to make sure no mistakes were made in constructing the request

                //if Order.CustomerId not null then Order.Customer.Id should not be null, and no creation of customer 
                if (orderDtoToCreate.CustomerId != null && orderDtoToCreate.CustomerId != orderDtoToCreate.Customer.Id)
                {
                    _logger.LogWarning("Customer Id was mismatched, difference between Order.CustomerId and Order.Customer.Id");
                    throw new CustomerNotFoundException("Error with provided customer.");
                }
                //validate Customer - if CustomerId == null then Customer.Id should be null
                else if (orderDtoToCreate.CustomerId == null && orderDtoToCreate.Customer.Id != null)
                {
                    _logger.LogWarning("Customer Id was mismatched, difference between Order.CustomerId and Order.Customer.Id");
                    throw new CustomerNotFoundException("Error with provided customer.");
                }
                //Test to see if the customer returned from the db using Order.CustomerId yields the same customer passed in, if not - client intention is likely not what they have asked for / at the very least the client provided incorrect data
                else if (orderDtoToCreate.CustomerId.HasValue && orderDtoToCreate.Customer.Id != null)
                {

                    var existingCustomerFromDb = await _customerAppService.GetCustomerNoTracking(orderDtoToCreate.CustomerId.Value);

                    var passedInCustomerAndCustomerIdMatch = ObjectComparer.AreObjectsEqual(orderDtoToCreate.Customer, existingCustomerFromDb);

                    if (!passedInCustomerAndCustomerIdMatch)
                        throw new CustomerNotFoundException("Error with provided customer.");

                }
                //else flag to create the customer in the transaction later
                else if (orderDtoToCreate.CustomerId == null && orderDtoToCreate.CustomerId == null)
                {
                    //if customer exists, different strategies exist here - but as you mention a guest checkout, add the customer details to the order to avoid data duplication /  to allow them to see their orders in the future if they choose to create an account
                    var existingCustomer = await _customerAppService.GetCustomer(orderDtoToCreate.Customer.Email);

                    if (existingCustomer != null)
                    {
                        orderDtoToCreate.CustomerId = existingCustomer.Id;
                        orderDtoToCreate.Customer = existingCustomer;
                    }
                }
                //cater for other eventuality that doesn't fit as a solution to the problem domain
                else
                {
                    _logger.LogWarning("Customer Id was mismatched, discrepency between Order.CustomerId and Order.Customer.Id");
                    throw new CustomerNotFoundException("Error with provided customer.");
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomerNotFoundException)
                {
                    _logger.LogWarning(ex, "Customer not found error.");
                    return BadRequest(ex.Message);
                }
                else
                {
                    _logger.LogError(ex, "An error occurred while creating the customer order.");
                    return StatusCode(500, ExceptionMessages.DefaultExceptionMessage);
                }
            }
            #endregion

            #region Validate OrderItems
            try
            {
                if (orderDtoToCreate.OrderItems == null || orderDtoToCreate.OrderItems.Any(oi => oi.Product == null) || orderDtoToCreate.OrderItems.Any(oi => oi.ProductId == 0))
                    throw new Exception("Problem with the OrderItems on the Order");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            #endregion

            IEnumerable<ProductDtoCreateOrder> listOfAllRequiredProducts;
            #region Validate Products
            //Validate Products against each OrderItem exist / exist as th3 customer expected
            try
            {

                listOfAllRequiredProducts = orderDtoToCreate.OrderItems.Select(oi => oi.Product);
                IEnumerable<ProductDtoCreateOrder> allMatchingProducts;
                allMatchingProducts = await _productAppService.GetProducts(listOfAllRequiredProducts.Select(pr => pr.Id));

                if (listOfAllRequiredProducts.Count() != allMatchingProducts.Count())
                    return BadRequest("One or more products in the order are not available.");

                //A check to make sure products requested match what exists in the database, to prove customer intention. This includes a price check which the client cannot be trusted with the responsibility for providing accurately
                //The AreCollectionsOfObjectsEqual function was setup to manage comparing with collections whilst omitting properties - like e.g. quantity (which changes all the time), but quanity in this case would be managed within Stock, not on Product (there is disconnect between them in real life), a product 'exists' but the stock for it might not
                var allRequestProductsMatchDbProducts = ObjectComparer.AreCollectionsOfObjectsEqual(listOfAllRequiredProducts.OrderBy(prod => prod.Id), allMatchingProducts.OrderBy(prod => prod.Id));
                if (!allRequestProductsMatchDbProducts)
                    return BadRequest("One or more products in the order are not available.");

            }
            catch (Exception ex)
            {
                if (ex is ProductNotFoundException)
                {
                    _logger.LogWarning(ex, "Product validation failed.");
                    return BadRequest(ex.Message);
                }
                else
                {
                    _logger.LogError(ex, "Product validation failed");
                    return StatusCode(500, ExceptionMessages.DefaultExceptionMessage);
                }
            }
            #endregion

            #region Validate Order
            //Check the price per product * qty for all orderitems == order total price (all product price checked above - products sent against the products in the database)

            var orderPriceEqualsSumOfOrderItems = _orderAppService.CheckOrderTotalPriceEqualsSumOfOrderItems(orderDtoToCreate.OrderTotalPrice, orderDtoToCreate.OrderItems);

            if (!orderPriceEqualsSumOfOrderItems)
            {
                //Log as security concern
                _logger.LogWarning($@"Pricing issue, {JsonConvert.SerializeObject(orderDtoToCreate)}");
                return BadRequest("There was a problem with the order");
            }

            #endregion

            //FUTURE CONSIDERATION: use concurrency check (RowVersion timestamp) to manage customer experience / UX, 

            #region StockItems check and stoc kallocation

            try
            {
                //get allStockItems for all product
                var stockItemsForAllProducts = await _stockItemAppService.GetStockItemsForProducts(listOfAllRequiredProducts.Select(pr => pr.Id));

                //check that qty is Ok for each 
                foreach (var orderItem in orderDtoToCreate.OrderItems)
                {
                    var allStockForProduct = stockItemsForAllProducts.Where(si => si.ProductId == orderItem.ProductId);

                    if (allStockForProduct.Count() < orderItem.Quantity)
                        throw new StockNotEnoughException($@"Not enough stock for product {orderItem.Product.Name}");

                    //assign the stockItems to the orderToCreate
                    orderItem.StockItems = allStockForProduct.Take(orderItem.Quantity);

                    //set the foreign IDs correctly
                    foreach (var stockItem in orderItem.StockItems)
                    {
                        stockItem.OrderItemId = orderItem.Id;
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex is StockNotEnoughException)
                {
                    _logger.LogWarning(ex, "Not enough stock to fulfil order");
                    return BadRequest(ex.Message);
                }
                else
                {
                    _logger.LogError(ex, "Stock allocation failed");
                    return StatusCode(500, ExceptionMessages.DefaultExceptionMessage);
                }
            }
            #endregion

            #region Create Order, OrderItems, optional Customer
            //create order > create customer if needed > create OrderLines > manage stock in here
            try
            {
                var createdCustomer = await _orderAppService.CreateOrderAndOptionalCustomer(orderDtoToCreate);
                return Ok(createdCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating order.");
                return StatusCode(500, ExceptionMessages.DefaultExceptionMessage);
            }
            #endregion
        }
    }
}
