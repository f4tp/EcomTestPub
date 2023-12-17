using EcomTest.Domain.DomainEntities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomTest.Domain.DomainEntities
{
    [Table("OrderItem")]
    public class OrderItem : Entity
    {

        [Required]
        [Column(TypeName = "int")]
        public int? Quantity { get; private set; }

        /// <summary>
        /// Total price of product cost * orderitem quantity
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "OrderItemTotalPrice must be a non-negative value.")]
        public decimal? OrderItemTotalPrice { get; private set; }

        //Foreign keys
        [Required]
        [Column(TypeName = "BIGINT")]
        public long? OrderId { get; private set; }

        [Required]
        [Column(TypeName = "BIGINT")]
        public long? ProductId { get; private set; }


        // Navigation properties
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; private set; }


        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; private set; }


        //OrderItem has multiple StockItems (as quantity managed in here)
        //Collection is made private and is modified incrementally only by the methods below, to misimise risk of accidents / impact of them
        private List<StockItem> _stockItems;
        public IEnumerable<StockItem> StockItems
        {
            get { return _stockItems; }
        }


        //no customer nav property as it is epected the customer will be looked up against the Order rather than the OrderItem


        private OrderItem() { } // Private constructor for EF

        // Constructor to create an OrderItem and associate it with an Order
        public OrderItem(int quantity, decimal orderItemTotalPrice, Product product, Order order)
        {
            // Add validation logic if needed
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.");

            if (orderItemTotalPrice !> 0)
                throw new ArgumentException("Order Item Total Price must be greater than 0.");

            //FUTURE CONSIDERATION: Add Business logic on here to make sure OrderItemTotalPrice is in tolerance - needs to be around Quantity * ProductPrice but can add logic to make sure it fits in with business requirements

            _stockItems = new List<StockItem>();

            OrderItemTotalPrice = orderItemTotalPrice;
            Quantity = quantity;

            Order = order;
            Product = product;

        }


        public StockItem AddStockItem(StockItem stockItem)
        {
            if(Product == null || Product.BasePrice !> 0)
                throw new ArgumentException("Problem with Product or its BasePrice");

            if (Quantity == null || Product.BasePrice! > 0)
                throw new ArgumentException("Problem with OrderItem Quantity");

            Quantity += 1;
            OrderItemTotalPrice = Product?.BasePrice * Quantity;
            _stockItems.Add(stockItem);
            return stockItem;
        }

        public void RemoveStockItem(StockItem stockItem)
        {
            if(Quantity == 0)
                throw new ArgumentException("Cannot reduce qty below 0");
            Quantity -= 1;
            OrderItemTotalPrice = Product?.BasePrice * Quantity ?? 0;
            _stockItems.Remove(stockItem);
        }
    }
}
