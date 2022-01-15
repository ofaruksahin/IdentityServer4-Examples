using System.Collections.Generic;
using IdentityServer.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetProducts()
        {
            var products = new List<Product>()
            {
                new Product
                {
                     Id = 1,
                     Name = "Ürün 1",
                     Price = 100,
                     Stock = 500
                },
                new Product
                {
                    Id = 2,
                    Name = "Ürün 2",
                    Price = 250,
                    Stock = 750
                }
            };
            return Ok(products);
        }

        [HttpPost]
        [Authorize(policy: "UpdateOrCreate")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok();
        }
    }
}
