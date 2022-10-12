using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.ViewModel
{
    public class ColourVM
    {
        public Guid ArticleID { get; set; }
        public Guid ColourID { get; set; }
        public string ArticleName { get; set; }=String.Empty;
        public string ColourCode { get; set; } = String.Empty;
        public string ColourName { get; set; }=String.Empty;
    }
}
