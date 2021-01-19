using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultApiClientServiceController.Entity;
using DefaultApiClientServiceController.Interface;

namespace DefaultApiClientServiceController.Service
{
    public abstract class BaseDataService<T, T2> : IBaseDataService<T, T2> where T : BaseEntity<T2>
    {
        protected readonly DbContext db;

        protected BaseDataService(DbContext db) { this.db = db; }

        #region Implementation of IBaseDataService<T>

        /// <summary> Get count of entities in database </summary>
        /// <returns>int count</returns>
        public async Task<int> GetTotalCountAsync() => await db.Set<T>().CountAsync();
        /// <summary> Get count of entities in database </summary>
        /// <returns>int count</returns>
        public virtual int GetTotalCount() => GetTotalCountAsync().Result;

        ///// <summary> Get all entities of T type from database </summary>
        ///// <returns>IEnumerable Entities</returns>
        //public virtual async Task<IQueryable<T>> GetAllAsync() => db.Set<T>().AsQueryable();
        /// <summary> Get all entities of T type from database </summary>
        /// <returns>IEnumerable Entities</returns>
        //public virtual IQueryable<T> GetAll() => GetAllAsync().Result;
        public virtual IQueryable<T> GetAll() => db.Set<T>().AsQueryable();

        /// <summary> Get entity of T type from database by id</summary>
        /// <returns>IEnumerable Entities</returns>
        public virtual async Task<T> GetAsync(T2 id) => await db.Set<T>().FindAsync(id);
        /// <summary> Get entity of T type from database by id</summary>
        /// <returns>IEnumerable Entities</returns>
        public virtual T Get(T2 id) => GetAsync(id).Result;

        /// <summary> Add new entity to database </summary>
        /// <param name="data">entity</param>
        /// <returns>database entity with id</returns>
        public virtual async Task<T> AddNewAsync(T data)
        {
            await db.Set<T>().AddAsync(data);
            await db.SaveChangesAsync();
            return data;
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
            var data = await db.Set<T>().FindAsync(id);
            if (data == null)
            {
                return null;
            }

            db.Set<T>().Remove(data);
            await db.SaveChangesAsync();

            return data;
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
            if (!id.Equals(data.ID))
            {
                return null;
            }

            db.Entry(data).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                return data;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary> Update entity data in database </summary>
        /// <param name="id">entity id</param>
        /// <param name="data">entity data</param>
        /// <returns>entity after update</returns>
        public virtual T Update(T2 id, T data) => UpdateAsync(id, data).Result;

        /// <summary> Check database to exist entity </summary>
        /// <param name="id">entity id</param>
        /// <returns>bool result</returns>
        public virtual async Task<bool> ExistsAsync(T2 id) => await db.Set<T>().AnyAsync(e => e.ID.Equals(id));
        /// <summary> Check database to exist entity </summary>
        /// <param name="id">entity id</param>
        /// <returns>bool result</returns>
        public virtual bool Exists(T2 id) => ExistsAsync(id).Result;

        /// <summary> Save database changes if u need </summary>
        /// <returns>bool result</returns>
        public virtual async Task<bool> SaveChangesAsync()
        {
            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary> Save database changes if u need </summary>
        /// <returns>bool result</returns>
        public virtual bool SaveChanges() => SaveChangesAsync().Result;
    }
        #endregion
}