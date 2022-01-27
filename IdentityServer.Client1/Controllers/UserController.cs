using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc"); //IdentityServer üzerinde bulunan oturumu kapatmak için kullanılır.
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetRefreshToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            HttpClient httpClient = new HttpClient();

            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
            //refreshTokenRequest.ClientId = "Client1-MVC";
            refreshTokenRequest.ClientId = "client1-resource-owner";
            refreshTokenRequest.ClientSecret = "secret";
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = disco.TokenEndpoint;

            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                throw token.Exception;
            }

            var tokens = new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = token.IdentityToken
                },
                new AuthenticationToken
                {
                    Name =  OpenIdConnectParameterNames.AccessToken,
                    Value = token.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = token.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                }
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;

            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public IActionResult AdminAction()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "member")]
        public IActionResult MemberAction()
        {
            return View();
        }
    }
}
