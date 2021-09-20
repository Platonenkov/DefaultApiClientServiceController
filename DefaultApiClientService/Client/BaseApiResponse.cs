using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DefaultApiClientService.Client
{
    /// <summary>
    /// Базовый ответ сервиса
    /// </summary>
    /// <typeparam name="T">тип данных в ответе</typeparam>
    public class BaseApiResponse<T>
    {
        /// <summary> count of entities in response </summary>
        [JsonProperty("@odata.count")]
        public int Count { get; set; }
        /// <summary> values of entities in response </summary>
        [JsonProperty("value")]
        public T Value { get; set; }
    }
}
