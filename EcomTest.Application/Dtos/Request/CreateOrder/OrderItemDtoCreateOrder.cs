using EcomTest.Application.Dtos.Base;
using EcomTest.Domain.DomainEntities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcomTest.Application.Dtos
{
    public class OrderItemDtoCreateOrder : EntityDto
    {
        public long OrderId { get; set; }

        //Required and value must be at least 1, can tell client intention then (stops default set to 0 .Net behaviour)
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "CustomerId must be greater than 0")]
        public long ProductId { get; set; }

        //Don't want to be putting orders with a qty of 0 through as bloats db / causes maintenance issues
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Qty must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "OrderItemTotalPrice must be a non-negative value.")]
        public decimal OrderItemTotalPrice { get; set; }

        //Could be a calculated property if the business logic / validation is setup for this, then cannot be set, only gotten / derrived from properties set elsewhere (which also have control on them)
        //public decimal OrderItemTotalPrice => UnitPrice * Quantity;

        //left public setter as this gets set during Order Creation logic as well as throguh serialization
        //Navigation props
        [Required]
        public ProductDtoCreateOrder? Product { get; set; }

        public IEnumerable<StockItemDtoCreateOrder>? StockItems { get; set; }

    }
}
