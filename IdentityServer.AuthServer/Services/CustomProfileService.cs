using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repository;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace IdentityServer.AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private ICustomUserRepository _customUserRepository;

        public CustomProfileService(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }

        private async Task<CustomUser> GetUser(string subjectId)
        {
            if (!int.TryParse(subjectId, out int userId))
                return null;
            return await _customUserRepository.FindById(userId);
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId =  context.Subject.GetSubjectId();
            var user = await GetUser(subjectId);
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Email,user.Email),
              new Claim("name",user.UserName),
              new Claim("city",user.City),
            };

            if(user.Id == 1)
            {
                claims.Add(new Claim("role", "admin"));
            }
            else
            {
                claims.Add(new Claim("role", "customer"));
            }

            //context.IssuedClaims.AddRange(claims); Burada claimleri issued claims içerisine eklersem token içerisine ekler.
            context.AddRequestedClaims(claims); //Burada ise userinfo endpointi için çalışır, userinfo endpointine istek gelirse buradan alır.
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = await GetUser(subjectId) != null;
        }
    }
}
