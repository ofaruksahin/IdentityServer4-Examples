using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Client1.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            string url = "https://localhost:5001";
            HttpClient httpClient = new HttpClient();
            var disco = await httpClient.GetDiscoveryDocumentAsync(url);

            if(disco.IsError)
            {
                throw disco.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest();
            passwordTokenRequest.Address = disco.TokenEndpoint;
            passwordTokenRequest.UserName = loginViewModel.Email;
            passwordTokenRequest.Password = loginViewModel.Password;
            passwordTokenRequest.ClientId = "client1-resource-owner";
            passwordTokenRequest.ClientSecret = "secret";

            var token = await httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                return View();
            }

            var userInfoRequest = new UserInfoRequest();
            userInfoRequest.Address = disco.UserInfoEndpoint;
            userInfoRequest.Token = token.AccessToken;

            var userInfo = await httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims,"Cookies","name","role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = token.IdentityToken
                },
                new AuthenticationToken
                {
                     Name = OpenIdConnectParameterNames.AccessToken,
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
                    Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)
                }
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal,authenticationProperties);

            return RedirectToAction("Index","User");
        }
    }
}
