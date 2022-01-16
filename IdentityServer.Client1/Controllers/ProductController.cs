using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Client1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ClientOptions _clientOptions;

        public ProductController(ClientOptions clientOptions)
        {
            _clientOptions = clientOptions;
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

            httpClient.SetBearerToken(token.AccessToken);
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
    }
}
