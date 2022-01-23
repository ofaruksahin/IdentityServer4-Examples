using System.Linq;
using System.Threading.Tasks;
using IdentityServer.AuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private CustomDbContext _dbContext;

        public CustomUserRepository(CustomDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _dbContext.CustomUsers.FirstOrDefaultAsync(f => f.Email == email);
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _dbContext.CustomUsers.FindAsync(id);
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _dbContext.CustomUsers.AnyAsync(f => f.Email == email && f.Password == password);
        }
    }
}
