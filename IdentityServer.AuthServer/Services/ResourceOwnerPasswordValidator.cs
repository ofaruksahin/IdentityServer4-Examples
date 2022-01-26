using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.AuthServer.Repository;
using IdentityServer4.Validation;

namespace IdentityServer.AuthServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ICustomUserRepository _customUserRepository;

        public ResourceOwnerPasswordValidator(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var isValid = await _customUserRepository.Validate(context.UserName, context.Password);

            if (isValid)
            {
                var user = await _customUserRepository.FindByEmail(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString(),OidcConstants.AuthenticationMethods.Password);
            }

        }
    }
}
