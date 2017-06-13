using System.IdentityModel.Tokens.Jwt;
using ShoppingCartApi.Extensions;
using ShoppingCartApi.Models;
using ShoppingCartApi.Options;
using ShoppingCartApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private TokenOptions Options { get; }

        public AuthenticationController(IOptions<TokenOptions> options)
        {
            Options = options.Value;
        }

        [HttpPost("[action]")]
        public IActionResult Token([FromBody]TokenRequest request)
        {
            // TODO: Authenticate request
            bool passed = true;
            if(passed){
                return Ok(TokenManager.BuildToken(Options));
            }else{
                return Unauthorized();
            }
        }
    }
}