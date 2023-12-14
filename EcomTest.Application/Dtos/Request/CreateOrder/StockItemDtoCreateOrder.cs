using EcomTest.Application.Dtos.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Application.Dtos
{
    public class StockItemDtoCreateOrder : EntityDto
    {
        //FUTURE CONSIDERATION: Work out how a private setter would operate within EF's auto check on this
        //Has Concurrency check performed
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        [Required]
        public string? ManufacturerCode { get; set; }

        // Foreign keys
        public long? ProductId { get; set; }

        public long? OrderItemId { get; set; }
    }
}
