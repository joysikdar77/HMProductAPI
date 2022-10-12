using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Entities
{
    public class ColourEntity
    {
        [JsonProperty("colourId")]
        public Guid colourId { get; set; }
        [JsonProperty("colourName")]
        public string colourName { get; set; } = String.Empty;
        [JsonProperty("colourCode")]
        public string colourCode { get; set; } = String.Empty;
    }
}
