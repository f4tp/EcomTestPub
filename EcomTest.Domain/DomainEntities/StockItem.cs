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
        public string ManufacturerCode { get; set; }

        [Required]
        [Column(TypeName = "BIGINT")]
        public long ProductId { get; set; }

        /// <summary>
        /// Null value in DB means this stock hasn't been allocated yet
        /// </summary>
        [Column(TypeName = "BIGINT")]
        public long? OrderItemId { get; set; }

        // Navigation props
        public Product? Product { get; set; }

        public OrderItem? OrderItem { get; set; }

        private StockItem() { } // Private constructor for EF

        public StockItem(string manufacturerCode, Product product, OrderItem orderItem)
        {
            ManufacturerCode = manufacturerCode;
            Product = product;
            OrderItem = orderItem;
        }
    }
}
