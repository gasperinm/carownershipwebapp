using CarOwnershipWebApp.Models;
using CarOwnershipWebApp.Models.Block;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public class OutcallService : IOutcallsService
    {
        public async Task<T> Get<T>(string uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return default(T);
        }

        public async Task<T> Post<T>(string uri, string data)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await httpClient.PostAsync(uri, new StringContent(data, Encoding.UTF8, "application/json"));

            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(content);
            }

            return default(T);
        }
    }
}
