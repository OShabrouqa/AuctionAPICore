using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("generate")]
        public IActionResult GenerateToken()
        {
            var token = _tokenService.GenerateToken();
            return Ok(new { Token = token });
        }
    }
}
