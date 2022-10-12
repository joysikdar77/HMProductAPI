using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.ViewModel
{
    public class StatusVM
    {
        public HttpStatusCode statusCode { get; set; }
        public string statusMessage { get; set; } = String.Empty;
    }
}
