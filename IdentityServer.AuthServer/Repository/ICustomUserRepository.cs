using System.Threading.Tasks;
using IdentityServer.AuthServer.Models;

namespace IdentityServer.AuthServer.Repository
{
    internal interface ICustomUserRepository
    {
        Task<bool> Validate(string email, string password);
        Task<CustomUser> FindById(int id);
        Task<CustomUser> FindByEmail(string email);
    }
}
