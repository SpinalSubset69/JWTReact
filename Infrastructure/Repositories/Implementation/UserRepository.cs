using API.Models;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementation
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
            :base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> SaveAndGetUser(User user)
        {
            _context.Add(user);
            user.Id = await _context.SaveChangesAsync();

            return user;
        }
    }
}
