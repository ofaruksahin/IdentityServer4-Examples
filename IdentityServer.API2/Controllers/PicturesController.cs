using System.Collections.Generic;
using IdentityServer.API2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetPictures()
        {
            var pictures = new List<Picture>()
            {
                new Picture
                {
                     Name = "Picture1",
                      Url = "http://picture1.com"
                }
            };
            return Ok(pictures);
        }
    }
}
