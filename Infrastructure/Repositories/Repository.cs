using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly UserContext _userContext;
        internal DbSet<T> _dbSet;
        public Repository(UserContext userContext)
        {
            this._userContext = userContext;
            this._dbSet = _userContext.Set<T>();
        }
        public void Create(T entity)
        {
            _dbSet.Add(entity); 
        }

        public async Task<T> FindByIdAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(criteria);
            return await query.FirstOrDefaultAsync<T>();
        }
    }
}
