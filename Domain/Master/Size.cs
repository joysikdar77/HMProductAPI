using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Master
{
    public class Size
    {
        public string productCode { get; set; } = String.Empty;
        [Key]
        public Guid SizeID { get; set; }
    }
}
