using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.DomainOptionalContracts
{
    public interface IHasConcurrencyCheck
    {
        [Timestamp]
        byte[] RowVersion { get; set; }
    }
}
