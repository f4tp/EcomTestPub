using EcomTest.Domain.DomainEntities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomTest.Domain.DomainEntities
{
    [Table("Order")]
    public class Order : Entity
    {
        //Made nullable as the default behaviour of a non-nullable is to instantiate the date to DateTime.MinValue which is not favourable in this context
        [Required]
        public DateTime? OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Order Total Price must be 0 or greater")]
        public decimal OrderTotalPrice { get; set; }

        //As above, default behaviour is to set this to 0 if not provided
        [Required]
        public long? CustomerId { get; set; }

        // Navigation props

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        //Collection is made private and is modified incrementally only by the methods below, to misimise risk of accidents / impact of them
        private List<OrderItem> _orderItems;

        //setter is taken out so dev can't wipe out the orderitems by mistake, as linked to db through ORM (e.g. EF), this minimises risk of OrderItems being handled incorrectly
        public IEnumerable<OrderItem> OrderItems{ get { return _orderItems; } }


        //Products will only ever need to be gotten through a lookup via EF, these wouldn't need to be added against an Order (they are added against an OrderItem) - this prop would then just be used for a backwards lookup so no need to add / remove, but here for correct relationship
        public IEnumerable<Product> Products { get; }

        private Order() { } // Private constructor for EF

        public Order(DateTime orderDate, Customer customer, decimal orderTotalPrice) //parameterised constructor
        {
            //Order needs a Customer but not necessarily a CustomerId - they might not have been persisted yet
            if (customer == null)
                throw new ArgumentException("Customer is required");

            //As this is a constructor, order price needs to be 0 or greater, never below... OrderDto accepting the request needs to be greater than zero as the it will include the OrderItems
            if(orderTotalPrice <= 0)
                throw new ArgumentException("Order Total Price needs to 0 or greater");

            //Instantiate Order Items List
            _orderItems = new List<OrderItem>();

            //Instantiate Order with passed in props
            OrderDate = orderDate;
            OrderTotalPrice = orderTotalPrice;
            CustomerId = customer.Id;
            Customer = customer;
        }

        public OrderItem AddOrderItem(int quantity, decimal orderItemTotalPrice, Product product)
        {
            var orderItemToAdd = new OrderItem(quantity, orderItemTotalPrice, product, this);
            _orderItems.Add(orderItemToAdd);
            OrderTotalPrice += orderItemTotalPrice;
            return orderItemToAdd;
        }

        public void RemoveOrderItem(int quantity, decimal orderItemTotalPrice, Product product)
        {
            var orderItemToRemove = new OrderItem(quantity, orderItemTotalPrice, product, this);
            OrderTotalPrice -= orderItemTotalPrice;
            _orderItems.Remove(orderItemToRemove);
        }
    }
}
