using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EcomTest.Application.Dtos.Base;

namespace EcomTest.Application.Dtos
{
    //this class doesn't extend EntityDto as need to manually add the Id in to add specific validation logic, Customer should be provided so checked the Id is > 0 and not null to swerve default behaviour of .Net (set to 0)
    public class CustomerDtoCreateOrder //: EntityDto
    {
        public long? Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
