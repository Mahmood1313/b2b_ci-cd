using B2BPriceAdmin.Core.Common.PagedResponse;
using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B2BPriceAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<Response<AuthResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.Authenticate(loginDTO);
            return response;
        }
    }
}
