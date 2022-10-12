using Application.Common.Entities;
using Application.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Service
{
    public class ExternalAPIService : IExternalAPIService
    {
        public ColourEntity GetColourEntity(string apiUrl, Guid colourId)
        {
            ColourEntity colourEntity = new ColourEntity();
            var client = createClient(apiUrl);
            var response = client.GetAsync("colourId=" + colourId ).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var colourResponse = response.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list  
                colourEntity =  JsonConvert.DeserializeObject<ColourEntity>(colourResponse);
            }
            return colourEntity;
        }

        public SizeEntity GetSizeEntity(string apiUrl, Guid sizeId)
        {
            SizeEntity sizeEntity = new SizeEntity();
            var client = createClient(apiUrl);
            var response = client.GetAsync("sizeScaleId=" + sizeId).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var colourResponse = response.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list  
                sizeEntity = JsonConvert.DeserializeObject<SizeEntity>(colourResponse);
            }
            return sizeEntity;
        }

        private HttpClient createClient(string apiUrl)
        {
            var client = new HttpClient();
            
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
