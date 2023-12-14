using EcomTest.Application.Dtos.Base;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcomTest.Application.Dtos
{
    public class OrderDtoCreateOrder : EntityDto
    {
        //Required and nullable - default behaviour is to set to DateTime.MinValue which woudl cause errors, needs to be provided by the client when user submits order
        [Required]
        public DateTime? OrderDate { get; set; }

        [Required]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "OrderTotalPrice must be a non-negative value.")]
        public decimal OrderTotalPrice { get; set; }

        //Nullable with a min value of 1; Null because not required - Customer might not exist yet so will need creating. Default .Net behaviour is to set to 0 so need to understand client behaviour here (either don't provide it for creation = null, or do provide it if there is a customer already created & is greater than 0)

        [Range(1, long.MaxValue, ErrorMessage = "CustomerId must be greater than 0")]
        public long? CustomerId { get; set; }

        //Left as public set here as OrderCreation routine sets these as well as JSON serialization

        //customer is expected to be passed in during the Create Order problem domain - eiother for for validation purposes (make sure Id passed in and customer gotten match up), or for creating a new one if CustomerId == null
        [Required]
        public CustomerDtoCreateOrder Customer { get; set; }

        //order items required as an order cannot be created without them
        [Required]
        public IEnumerable<OrderItemDtoCreateOrder> OrderItems { get; set; }
    }
}
