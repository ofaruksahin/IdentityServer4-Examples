using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Client1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ClientOptions _clientOptions;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;

        public ProductController(
            ClientOptions clientOptions,
            IApiResourceHttpClient apiResourceHttpClient
            )
        {
            _clientOptions = clientOptions;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = new HttpClient();

            //Burada discovery endpointte istek yapan ve parse eden bir extension method mevcut
            var disco = await httpClient.GetDiscoveryDocumentAsync(_clientOptions.AuthorityUrl); 

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            //Burada token almak için ihtiyacım olan parameterleri bir model classında tanımlıyorum.
            //Burada ihtiyacım olan tokeni nereden alacağım, client id nedir, client secreti nedir
            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();
            clientCredentialsTokenRequest.Address = disco.TokenEndpoint; //Token endpoint
            clientCredentialsTokenRequest.ClientId = _clientOptions.ClientId; //Client Id
            clientCredentialsTokenRequest.ClientSecret = _clientOptions.ClientSecret; //Client Secret

            var token = await httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (token.IsError)
            {
                throw token.Exception;
            }

            //httpClient.SetBearerToken(token.AccessToken);
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            httpClient.SetBearerToken(accessToken);
            var response = await httpClient.GetAsync("https://localhost:5021/api/products/GetProducts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine(content);
            }
            else
            {

            }

            return View();
        }

        public async Task<IActionResult> Index2()
        {
            var client = await _apiResourceHttpClient.GetHttpClient();
            var response = await client.GetAsync("https://localhost:5021/api/products/GetProducts");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            return Ok(new { });
        }
    }
}
