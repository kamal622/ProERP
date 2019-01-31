using System.Collections.Generic;
using System.Linq;

namespace ProERP.Core.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : BaseEntity
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Insert(IEnumerable<T> entities);
        //void Update(IEnumerable<T> entities);
        void Update(IEnumerable<T> entities);
        void Delete(IEnumerable<T> entities);
        IQueryable<T> Table { get; }
    }
}
