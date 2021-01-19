using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DefaultApiClientService.Client;
using DefaultApiClientServiceController.Entity;
using DefaultApiClientServiceController.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DefaultApiClientServiceController.Client
{
    /// <summary>
    /// Base service client
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="T2">Entity Id - type (int, long, Guid)</typeparam>
    public abstract class BaseApiClient<T,T2> : BaseClient, IBaseDataService<T,T2> where T : BaseEntity<T2>, new()
    {
        protected BaseApiClient(IConfiguration configuration, string ServiceAddress) : base(configuration, ServiceAddress)
        {
        }

        public async Task<BaseApiResponse<T>> GetAsync(int skip = 0, int top = 0)
        {
            var response = _Client.GetAsync($"{ServiceAddress}?$count=true&$skip={skip}&$top={top}").Result;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BaseApiResponse<T>>(json);
            }

            return new BaseApiResponse<T>();
        }

        #region Implementation of IBaseDataService

        /// <summary> Get count of entities in database </summary>
        /// <returns>int count</returns>
        public virtual async Task<int> GetTotalCountAsync() => await GetAsync<int>($"{ServiceAddress}/Count");
        /// <summary> Get count of entities in database </summary>
        /// <returns>int count</returns>
        public virtual int GetTotalCount() => GetTotalCountAsync().Result;

        /// <summary> Get all entities of T type from database </summary>
        /// <returns>IEnumerable Entities</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync() => await GetAsync<List<T>>($"{ServiceAddress}");
        /// <summary> Get all entities of T type from database </summary>
        /// <returns>IEnumerable Entities</returns>
        public virtual IEnumerable<T> GetAll() => Get<List<T>>($"{ServiceAddress}");

        /// <summary> Get entity of T type from database by id</summary>
        /// <returns>IEnumerable Entities</returns>
        public virtual async Task<T> GetAsync(T2 id) => await GetAsync<T>($"{ServiceAddress}/{id}");
        /// <summary> Get entity of T type from database by id</summary>
        /// <returns>IEnumerable Entities</returns>
        public virtual T Get(T2 id) => GetAsync(id).Result;

        /// <summary> Add new entity to database </summary>
        /// <param name="data">entity</param>
        /// <returns>database entity with id</returns>
        public virtual async Task<T> AddNewAsync(T data)
        {
            var result = await PostAsync(ServiceAddress, data);
            if (!result.IsSuccessStatusCode) return null;
            var entity = await result.Content.ReadAsAsync<T>();
            return entity;
        }

        /// <summary> Add new entity to database </summary>
        /// <param name="data">entity</param>
        /// <returns>database entity with id</returns>
        public virtual T AddNew(T data) => AddNewAsync(data).Result;

        /// <summary> Delete entity from database </summary>
        /// <param name="id">entity id</param>
        /// <returns>database entity which was deleted</returns>
        public virtual async Task<T> DeleteAsync(T2 id)
        {
            var result = await DeleteAsync($"{ServiceAddress}/{id}");
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var entity = await result.Content.ReadAsAsync<T>();
            return entity;
        }

        /// <summary> Delete entity from database </summary>
        /// <param name="id">entity id</param>
        /// <returns>database entity which was deleted</returns>
        public virtual T Delete(T2 id) => DeleteAsync(id).Result;

        /// <summary> Update entity data in database </summary>
        /// <param name="id">entity id</param>
        /// <param name="data">entity data</param>
        /// <returns>entity after update</returns>
        public virtual async Task<T> UpdateAsync(T2 id, T data)
        {
            var response = await PutAsync($"{ServiceAddress}/{id}", data);
            var entity = await response.Content.ReadAsAsync<T>();
            return entity;
        }

        /// <summary> Update entity data in database </summary>
        /// <param name="id">entity id</param>
        /// <param name="data">entity data</param>
        /// <returns>entity after update</returns>
        public virtual T Update(T2 id, T data) => UpdateAsync(id, data).Result;

        /// <summary> Check database to exist entity </summary>
        /// <param name="id">entity id</param>
        /// <returns>bool result</returns>
        public virtual async Task<bool> ExistsAsync(T2 id) => await GetAsync<bool>($"{ServiceAddress}/Exist/{id}");
        /// <summary> Check database to exist entity </summary>
        /// <param name="id">entity id</param>
        /// <returns>bool result</returns>
        public virtual bool Exists(T2 id) => ExistsAsync(id).Result;

        /// <summary> Save database changes if u need </summary>
        /// <returns>bool result</returns>
        public virtual async Task<bool> SaveChangesAsync() => await GetAsync<bool>($"{ServiceAddress}/SaveChanges");

        /// <summary> Save database changes if u need </summary>
        /// <returns>bool result</returns>
        public virtual bool SaveChanges() => SaveChangesAsync().Result;

        #endregion

    }
}
