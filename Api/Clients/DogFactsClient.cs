using Api.DTOs;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Clients
{
    public class DogFactsClient
    {
        private const string BaseUri = "https://dog-api.kinduff.com/api";
        private static readonly HttpClient _httpClient = new HttpClient();
        
        public async Task<DogFactsDto> GetFactsAsync(int count)
        {
            using (var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseUri}/facts?number={count}"),
            })
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new Exception();

                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<DogFactsDto>(content);
                }
            }
        }
    }
}
