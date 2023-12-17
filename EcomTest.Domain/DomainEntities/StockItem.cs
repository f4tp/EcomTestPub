using EcomTest.Domain.DomainEntities.Base;
using EcomTest.DomainOptionalContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomTest.Domain.DomainEntities
{
    [Table("StockItem")]
    public class StockItem : Entity, IHasConcurrencyCheck
    {

        //FUTURE CONSIDERATION: Work out how a private setter would operate within EF's auto check on this
        //Has Concurrency check performed
        [Required]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public string ManufacturerCode { get; private set; }

        [Required]
        [Column(TypeName = "BIGINT")]
        public long ProductId { get; private set; }

        /// <summary>
        /// Null value in DB means this stock hasn't been allocated yet
        /// </summary>
        [Column(TypeName = "BIGINT")]
        public long? OrderItemId { get; private set; }

        // Navigation props
        public Product? Product { get; private set; }

        public OrderItem? OrderItem { get; private set; }

        private StockItem() { } // Private constructor for EF

        public StockItem(string manufacturerCode, Product product, OrderItem orderItem)
        {
            OrderItemId = orderItem.Id;
            ProductId = product.Id;
            ManufacturerCode = manufacturerCode;
            Product = product;
            OrderItem = orderItem;
        }
    }
}
