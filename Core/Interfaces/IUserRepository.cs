using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> SaveAndGetUser(User user);   
        Task<User> GetUserByEmailAsync(string email);   
    }
}
