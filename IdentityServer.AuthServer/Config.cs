using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("resource_api1")
                {
                    Scopes = new List<string>{"api1.read","api1.write","api1.update"},
                    ApiSecrets = new[]{new Secret("secretapi1".Sha256())}
                },
                new ApiResource("resource_api2")
                {
                    Scopes = new List<string>{"api2.read","api2.write","api2.update"},
                    ApiSecrets = new[]{new Secret("secretapi2".Sha256())}
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","" +
                "API 1 için güncelleme izni"),
                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Client1 Application",
                    ClientId = "Client1",
                    ClientSecrets = new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "api1.read"
                    }
                },
                new Client
                {
                    ClientName = "Client2 Application",
                    ClientId = "Client2",
                    ClientSecrets = new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "api1.read","api2.write","api2.update"
                    }
                },
                new Client
                {
                    ClientName = "Client1 Application (MVC)",
                    ClientId = "Client1-MVC",
                    ClientSecrets = new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:5011/signin-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1.read",
                        "CountryAndCity",
                        "Roles"
                    },
                    //SPA veya Mobil cihazlar örneklerinde oluşacak clientlarda bu senaryo testini yapacağım.
                    RequirePkce = false, //Require Proof Key For Code
                    AllowOfflineAccess = true, //Token ile birlikte refresh_token almak için kullanıyorum.
                    AccessTokenLifetime = (int)TimeSpan.FromHours(2).TotalSeconds, //access_token için 2 saatlik bir ömür belirledim.
                    RefreshTokenUsage = TokenUsage.ReUse, //Refresh token birden fazla kez kullanılabilir dedim
                    AbsoluteRefreshTokenLifetime = (int)TimeSpan.FromDays(60).TotalSeconds, //Refresh token için 60 gün bir ömür verdim.
                    RefreshTokenExpiration = TokenExpiration.Absolute, //refresh_token ömrü kullandıkça artmaması için bunu belirledim.
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:5011/signout-callback-oidc"
                    },
                    RequireConsent = true, //Onay sayfası çıkarmak için true yapıyoruz
                },
                new Client
                {
                    ClientName = "Client2 Application (MVC)",
                    ClientId = "Client2-MVC",
                    ClientSecrets = new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:5013/signin-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1.read",
                        "CountryAndCity",
                        "Roles"
                    },
                    //SPA veya Mobil cihazlar örneklerinde oluşacak clientlarda bu senaryo testini yapacağım.
                    RequirePkce = false, //Require Proof Key For Code
                    AllowOfflineAccess = true, //Token ile birlikte refresh_token almak için kullanıyorum.
                    AccessTokenLifetime = (int)TimeSpan.FromHours(2).TotalSeconds, //access_token için 2 saatlik bir ömür belirledim.
                    RefreshTokenUsage = TokenUsage.ReUse, //Refresh token birden fazla kez kullanılabilir dedim
                    AbsoluteRefreshTokenLifetime = (int)TimeSpan.FromDays(60).TotalSeconds, //Refresh token için 60 gün bir ömür verdim.
                    RefreshTokenExpiration = TokenExpiration.Absolute, //refresh_token ömrü kullandıkça artmaması için bunu belirledim.
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:5013 /signout-callback-oidc"
                    },
                    RequireConsent = true, //Onay sayfası çıkarmak için true yapıyoruz
                },
            };
        }

        //Token içerisinde kullanıcı ile ilgili hangi bilgiler olacak.
        public static IEnumerable<IdentityResource> GetIdentityResources() 
        {
            return new List<IdentityResource>
            {
                 new IdentityResources.OpenId(), //Token içerisinde kullanıcı idsi olmasını istediğimi belirtiyorum.
                 new IdentityResources.Profile(), //Kullanıcı profili ile ilgili bilgileri erişebilir onu belirtiyorum
                 new IdentityResource
                 {
                     Name = "CountryAndCity",
                     DisplayName = "Country and City",
                     Description = "User country and city info",
                     UserClaims =
                     {
                         "country","city"
                     }
                 },
                 new IdentityResource
                 {
                     Name = "Roles",
                     DisplayName = "Roles",
                     Description = "User Roles",
                     UserClaims =
                     {
                         "role"
                     }
                 }
            };
        }

        // InMemory'de saklayacağım test kullanıcıları oluşturmak için kullanıyorum.
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId  = "1", //Kullanıcı idsine karşılık gelecek
                    Username = "ofaruksahin@outlook.com.tr",
                    Password = "123",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name","Ömer Faruk"),
                        new Claim("family_name","Şahin"),
                        new Claim("country","Türkiye"),
                        new Claim("city","İstanbul"),
                        new Claim("role","admin"),
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "harunsahin@outlook.com.tr",
                    Password = "123",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name","Harun"),
                        new Claim("family_name","Şahin"),
                        new Claim("country","Türkiye"),
                        new Claim("city","İstanbul"),
                        new Claim("role","member")
                    }
                }
            };
        }
    }
}
