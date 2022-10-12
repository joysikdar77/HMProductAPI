using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Entities
{
    public class SizeEntity
    {
        [JsonProperty("sizeId")]
        public Guid sizeId { get; set; }
        [JsonProperty("sizeName")]
        public string sizeName { get; set; } = String.Empty;
    }
}
