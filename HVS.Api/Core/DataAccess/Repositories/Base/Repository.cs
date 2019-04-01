using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HVS.Api.Core.Entities;

namespace HVS.Api.Core.DataAccess.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        #region Protected Properties

        protected HVSNetCoreDbContext DataContext { get; set; }

        #endregion

        public HVSNetCoreDbContext DbContext
        {
            get { return DataContext; }
        }

        public Repository(HVSNetCoreDbContext entities)
        {
            DataContext = entities;
        }

        #region Public Methods

        /// <summary>
        /// Get current db context
        /// </summary>
        /// <returns></returns>
        public HVSNetCoreDbContext GetDbContext()
        {
            return DbContext;
        }

        #endregion

        #region Get

        public virtual IQueryable<T> GetAll()
        {
            return DataContext.Set<T>().Where(i => !i.RecordDeleted).OrderBy(i => i.RecordOrder);
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> expression)
        {
            return GetAll().Where(expression).OrderBy(i => i.RecordOrder);
        }

        public async Task<T> FetchFirstAsync(Expression<Func<T, bool>> expression)
        {
            return await GetAll().FirstOrDefaultAsync(expression);
        }

        public async Task<T> GetByIdAsync(Guid? id)
        {
            return await DataContext.Set<T>().FirstOrDefaultAsync(i => i.Id == id);
        }

        #endregion

        /// <summary>
        /// Excute sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<ResponseModel> ExcuteSqlAsync(string sql)
        {
            var response = new ResponseModel();
            DataContext.Database.ExecuteSqlCommand(sql);
            await DataContext.SaveChangesAsync();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        #region Insert

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="recordActive"></param>
        /// <returns></returns>
        public async Task<ResponseModel> InsertAsync(T entity)
        {
            var response = new ResponseModel();

            var dbSet = DataContext.Set<T>();
            dbSet.Add(entity);
            await DataContext.SaveChangesAsync();
            response.Data = entity;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        /// <summary>
        /// Insert list of entity
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="recordActive"></param>
        /// <returns></returns>
        public async Task<ResponseModel> InsertAsync(IEnumerable<T> entities)
        {
            var response = new ResponseModel();
            var now = DateTime.UtcNow;

            var dbSet = DataContext.Set<T>();
            foreach (var entity in entities)
            {
                dbSet.Add(entity);
            }
            await DataContext.SaveChangesAsync();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        #endregion

        #region Delete

        public async Task<ResponseModel> DeleteAsync(T entity)
        {
            var response = new ResponseModel();

            if (entity == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }

            var dbSet = DataContext.Set<T>();
            dbSet.Remove(entity);
            await DataContext.SaveChangesAsync();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        public async Task<ResponseModel> DeleteAsync(IEnumerable<T> entities)
        {
            var response = new ResponseModel();

            if (entities == null || !entities.Any())
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }

            var dbSet = DataContext.Set<T>();
            dbSet.RemoveRange(entities);
            await DataContext.SaveChangesAsync();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        public async Task<ResponseModel> DeleteAsync(IEnumerable<Guid> ids)
        {
            var response = new ResponseModel();

            if (ids == null || !ids.Any())
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }

            var dbSet = DataContext.Set<T>();
            dbSet.RemoveRange(Fetch(e => ids.Contains(e.Id)));
            await DataContext.SaveChangesAsync();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        public async Task<ResponseModel> DeleteAsync(Guid id)
        {
            var response = new ResponseModel();

            var entity = await GetByIdAsync(id);
            var dbSet = DataContext.Set<T>();
            dbSet.Remove(entity);
            await DataContext.SaveChangesAsync();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        #endregion

        #region Update

        public async Task<ResponseModel> UpdateAsync(T entity)
        {
            var response = new ResponseModel();
            entity.UpdatedOn = DateTime.UtcNow;

            await DataContext.SaveChangesAsync();
            response.Data = entity.Id;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        public async Task<ResponseModel> SetRecordDeletedAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            entity.RecordDeleted = true;
            return await UpdateAsync(entity);
        }

        public async Task<ResponseModel> SetRecordInactiveAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            entity.RecordActive = false;
            return await UpdateAsync(entity);
        }

        #endregion
    }
}
