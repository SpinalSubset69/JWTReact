using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext _context;

        public UnitOfWork(UserContext context)            
        {
            _context = context;
            this.User = new UserRepository(context);
        }

        public IUserRepository User { get; private set; }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
