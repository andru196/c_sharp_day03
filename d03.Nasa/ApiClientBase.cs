using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace d03.Nasa
{
    public abstract class ApiClientBase
    {
        protected string ApiKey{get; init;}
        protected HttpClient _client;
        protected ApiClientBase(string apiKey)
        {
            _client = new HttpClient();
            ApiKey = apiKey;
        }

        protected async Task<T> HttpGetAsync<T>(string url)
        {
            var builder = new UriBuilder(url);
            if (string.IsNullOrEmpty(builder.Query))
                builder.Query = $"api_key={ApiKey}";
            else
                builder.Query += $"&api_key={ApiKey}";
            var response = await _client.GetAsync(builder.Uri);
            if (!response.IsSuccessStatusCode && response.ReasonPhrase == "Forbidden")
                throw new Exception("Access denide");
            else if (!response.IsSuccessStatusCode)
                throw new Exception("Not OK:\t" + response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() {PropertyNameCaseInsensitive = false});
            return data;
        }

        protected async Task<T> HttpGetAsync<T>(string url, string containerName)
        {
            var builder = new UriBuilder(url);
            if (string.IsNullOrEmpty(builder.Query))
                builder.Query = $"api_key={ApiKey}";
            else
                builder.Query += $"&api_key={ApiKey}";
            var response = await _client.GetAsync(builder.Uri);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Not OK:\t" + response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty(containerName, out var element))
            {
                content = element.GetRawText();
                var data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() {PropertyNameCaseInsensitive = false});
                return data;
            }
            else
                throw new Exception("Deserialize error!!!!!!!!!!");
        }
    }
}