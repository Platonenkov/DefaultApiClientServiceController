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
    /// <summary>
    /// Базовый клиент с реализациями
    /// </summary>
    public abstract class BaseClient
    {
        /// <summary>
        /// Http клиент
        /// </summary>
        protected readonly HttpClient _Client;
        /// <summary>
        /// адрес сервиса
        /// </summary>
        protected string ServiceAddress { get; }
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="configuration">конфигурация</param>
        /// <param name="ServiceAddress">адрес сервиса</param>
        protected BaseClient(IConfiguration configuration, string ServiceAddress)
        {
            this.ServiceAddress = ServiceAddress;
            _Client = new HttpClient
            {
                BaseAddress = new Uri(configuration["ClientAddress"])
            };

            _Client.DefaultRequestHeaders.Accept.Clear();
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Базовая реализация метода
        /// </summary>
        /// <typeparam name="T">Тип нужных данных</typeparam>
        /// <param name="url">адрес</param>
        /// <returns></returns>
        protected T Get<T>(string url) where T : new() => GetAsync<T>(url).Result;
        /// <summary>
        /// Базовая реализация метода
        /// </summary>
        /// <typeparam name="T">Тип нужных данных</typeparam>
        /// <param name="url">адрес</param>
        /// <returns></returns>
        protected async Task<T> GetAsync<T>(string url) where T : new()
        {
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

        /// <summary>
        /// Базовая реализация метода
        /// </summary>
        /// <typeparam name="T">Тип нужных данных</typeparam>
        /// <param name="url">адрес</param>
        /// <param name="item">данные</param>
        /// <returns></returns>
        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        /// <summary>
        /// Базовая реализация метода
        /// </summary>
        /// <typeparam name="T">Тип нужных данных</typeparam>
        /// <param name="url">адрес</param>
        /// <param name="item">данные</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            var response = await _Client.PostAsJsonAsync(url, item);
            return response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Базовая реализация метода
        /// </summary>
        /// <typeparam name="T">Тип нужных данных</typeparam>
        /// <param name="url">адрес</param>
        /// <param name="item">данные</param>
        /// <returns></returns>
        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            var response = await _Client.PutAsJsonAsync(url, item);
            return response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Базовая реализация метода
        /// </summary>
        /// <param name="url">адрес</param>
        /// <returns></returns>
        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        /// <summary>
        /// Базовая реализация метода
        /// </summary>
        /// <param name="url">адрес</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> DeleteAsync(string url) => await _Client.DeleteAsync(url);

    }
}
