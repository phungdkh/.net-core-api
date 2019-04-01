using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HVS.Api.Core.DataAccess.Repositories.Base
{
    public interface IRepository<T>
    {
        HVSNetCoreDbContext GetDbContext();

        #region Get

        IQueryable<T> GetAll();

        IQueryable<T> Fetch(Expression<Func<T, bool>> expression);

        Task<T> FetchFirstAsync(Expression<Func<T, bool>> expression);

        Task<T> GetByIdAsync(Guid? id);

        Task<ResponseModel> ExcuteSqlAsync(string sql);

        #endregion

        #region Insert

        Task<ResponseModel> InsertAsync(T entity);

        Task<ResponseModel> InsertAsync(IEnumerable<T> entities);

        #endregion

        #region Delete

        Task<ResponseModel> DeleteAsync(T entity);

        Task<ResponseModel> DeleteAsync(IEnumerable<T> entities);

        Task<ResponseModel> DeleteAsync(IEnumerable<Guid> ids);

        Task<ResponseModel> DeleteAsync(Guid id);

        #endregion

        #region Update

        Task<ResponseModel> UpdateAsync(T entity);

        Task<ResponseModel> SetRecordInactiveAsync(Guid id);

        Task<ResponseModel> SetRecordDeletedAsync(Guid id);

        #endregion
    }
}
