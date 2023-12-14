using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcomTest.Application.Dtos.Base;
using System.Text.Json.Serialization;

namespace EcomTest.Application.Dtos
{
    //this class doesn't extend EntityDto as need to manually add the Id in to add specific validation logic, product should be provided so checked in the Id is > 0 and not null to swerve default behaviour of .Net (set to 0)
    public class ProductDtoCreateOrder //: EntityDto
    {
        //Required / at least 1 because not creating this as part of Create order problem domain
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }

        //As above
        [Required]
        public string Name { get; set; }

        //As above
        [Required]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "BasePrice must be greater than 0")]
        public decimal BasePrice { get; set; }

        //OrderItems, although in the Product Entity, not needed here as not required for Create Order problem domain, and inclusion would mean bloated DTO with more liklihood of causing issue down the line
    }
}
