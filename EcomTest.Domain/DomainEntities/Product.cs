using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EcomTest.Domain.DomainEntities.Base;

namespace EcomTest.Domain.DomainEntities
{
    [Table("Product")]
    public class Product : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BasePrice { get; set; }


        //Navigation properties

        //As above, Orders wouldn't need adding against a Product but used for lookup purposes - a Product has multiple Orders
        public IEnumerable<Order> Orders { get; }

        private Product() { } // Private constructor for EF

        //Name and BasePrice can only be set on instantiation of a new instance, as well as through AutoMapper, methods need to be added to be able to do this if required, see below for example
        public Product(string name, decimal basePrice)
        {

            if (basePrice !> 0)
                throw new ArgumentException("Base Price must be greater than 0.");

            Name = name;
            BasePrice = basePrice;
        }
    }
}
