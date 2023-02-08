using B2BPriceAdmin.Core.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B2BPriceAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {

        }

        [HttpPost("CreateNewUser")]
        [HasPermission(Permissions.UserCreate)]
        public async Task<IActionResult> CreateNewUser()
        {
            return Ok();
        }
    }
}
