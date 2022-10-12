using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.ViewModel
{
    public class ProductVM
    {
        //can be done with data annotations like [Required("attribute")] .. But for a solid validation and proper HttpStatusCode doing a manual validation
        public string productCode { get; set; } = String.Empty;
        public Guid productID { get; set; }
        public string productName { get; set; } = String.Empty;
        public int productYear { get; set; }
        public int channelID { get; set; }
        public string createdBy { get; set; } = String.Empty;
        public DateTime createdDate { get; set; }
        public List<ColourVM> articles { get; set; } = new List<ColourVM>();
        public List<SizeVM> sizes { get; set; } = new List<SizeVM>();
    }
}
