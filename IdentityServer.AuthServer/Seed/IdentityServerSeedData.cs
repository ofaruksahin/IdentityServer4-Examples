using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityServer.AuthServer.Seed
{
    public static class IdentityServerSeedData
    {
        public static void Seed(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = Config.GetClients();
                foreach(var client in clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
            }

            if (!context.ApiResources.Any())
            {
                var apiResources = Config.GetApiResources();
                foreach(var apiResource in apiResources)
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }
            }

            if (!context.ApiScopes.Any())
            {
                var apiScopes = Config.GetApiScopes();
                foreach(var apiScope in apiScopes)
                {
                    context.ApiScopes.Add(apiScope.ToEntity());
                }
            }

            if (!context.IdentityResources.Any())
            {
                var identityResources = Config.GetIdentityResources();
                foreach (var identityResource in identityResources)
                {
                    context.IdentityResources.Add(identityResource.ToEntity());
                }
            }

            context.SaveChanges();
        }
    }
}
