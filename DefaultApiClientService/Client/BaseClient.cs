using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DefaultApiClientService.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DefaultApiClientServiceController.Client
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _Client;
        protected string ServiceAddress { get; }

        protected BaseClient(IConfiguration configuration, string ServiceAddress)
        {
            this.ServiceAddress = ServiceAddress;
            Console.WriteLine($"Service address: {ServiceAddress}");
            _Client = new HttpClient
            {
                BaseAddress = new Uri(configuration["ClientAddress"])
            };

            _Client.DefaultRequestHeaders.Accept.Clear();
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Odata

        //protected BaseApiResponse<T> GetOdata<T>(string url, int skip = 0, int top = 0) where T : new() => GetOdataAsync<T>(url, skip, top).Result;

        //protected async Task<BaseApiResponse<T>> GetOdataAsync<T>(string url, int skip = 0, int top = 0) where T : new()
        //{
        //    var response = await _Client.GetAsync($"{url}?$count=true&$skip={skip}&$top={top}");
        //    if (!response.IsSuccessStatusCode) return new BaseApiResponse<T>(){Values = new List<T>()};
        //    var result = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<BaseApiResponse<T>>(result);
        //}

        #endregion

        protected T Get<T>(string url) where T : new() => GetAsync<T>(url).Result;

        protected async Task<T> GetAsync<T>(string url) where T : new()
        {
            Console.WriteLine($"url: {url}");

            var response = await _Client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new T();
            try
            {
                return await response.Content.ReadAsAsync<T>();
            }
            catch (JsonSerializationException)
            {
                var row = await response.Content.ReadAsStringAsync();
                var d = JsonConvert.DeserializeObject<BaseApiResponse<T>>(row);
                return d.Value;
            }
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            var response = await _Client.PostAsJsonAsync(url, item);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            var response = await _Client.PutAsJsonAsync(url, item);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url) => await _Client.DeleteAsync(url);

    }
}
