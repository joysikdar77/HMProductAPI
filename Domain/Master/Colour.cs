using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Master
{
    public class Colour
    {
        public string productCode { get; set; } = String.Empty;
        [Key]
        public Guid ArticleID { get; set; }
        public Guid ColourID { get; set; }
    }
}
