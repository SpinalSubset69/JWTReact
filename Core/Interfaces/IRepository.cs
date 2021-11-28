using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Create(T entity);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria);
    }
}
