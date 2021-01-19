using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultApiClientServiceController.Interface
{
    /// <summary>
    /// Base service interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="T2">Entity Id - type (int, long, Guid)</typeparam>
    public interface IBaseDataService<T, in T2>
    {
        /// <summary> Get count of entities in database </summary>
        /// <returns>int count</returns>
        public Task<int> GetTotalCountAsync();
        /// <summary> Get count of entities in database </summary>
        /// <returns>int count</returns>
        public int GetTotalCount();
        /// <summary> Get all entities of T type from database </summary>
        /// <returns>IEnumerable Entities</returns>
        public Task<IEnumerable<T>> GetAllAsync();
        /// <summary> Get all entities of T type from database </summary>
        /// <returns>IEnumerable Entities</returns>
        public IEnumerable<T> GetAll();
        /// <summary> Get entity of T type from database by id</summary>
        /// <returns>IEnumerable Entities</returns>
        public Task<T> GetAsync(T2 id);
        /// <summary> Get entity of T type from database by id</summary>
        /// <returns>IEnumerable Entities</returns>
        public T Get(T2 id);
        /// <summary> Add new entity to database </summary>
        /// <param name="data">entity</param>
        /// <returns>database entity with id</returns>
        public Task<T> AddNewAsync(T data);
        /// <summary> Add new entity to database </summary>
        /// <param name="data">entity</param>
        /// <returns>database entity with id</returns>
        public T AddNew(T data);
        /// <summary> Delete entity from database </summary>
        /// <param name="id">entity id</param>
        /// <returns>database entity which was deleted</returns>
        public Task<T> DeleteAsync(T2 id);
        /// <summary> Delete entity from database </summary>
        /// <param name="id">entity id</param>
        /// <returns>database entity which was deleted</returns>
        public T Delete(T2 id);
        /// <summary> Update entity data in database </summary>
        /// <param name="id">entity id</param>
        /// <param name="data">entity data</param>
        /// <returns>entity after update</returns>
        public Task<T> UpdateAsync(T2 id, T data);
        /// <summary> Update entity data in database </summary>
        /// <param name="id">entity id</param>
        /// <param name="data">entity data</param>
        /// <returns>entity after update</returns>
        public T Update(T2 id, T data);
        /// <summary> Check database to exist entity </summary>
        /// <param name="id">entity id</param>
        /// <returns>bool result</returns>
        public Task<bool> ExistsAsync(T2 id);
        /// <summary> Check database to exist entity </summary>
        /// <param name="id">entity id</param>
        /// <returns>bool result</returns>
        public bool Exists(T2 id);
        /// <summary> Save database changes if u need </summary>
        /// <returns>bool result</returns>
        public Task<bool> SaveChangesAsync();
        /// <summary> Save database changes if u need </summary>
        /// <returns>bool result</returns>
        public bool SaveChanges();

    }
}
