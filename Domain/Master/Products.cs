using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Master
{
    public class Products
    {
        [Key]
        public string productCode { get; set; } = String.Empty;
        public Guid productID { get; set; }
        public string productName { get; set; } = String.Empty;
        public int productYear { get; set; }
        public int channelID { get; set; }
        public string CreatedBy { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; }

        
    }
}
