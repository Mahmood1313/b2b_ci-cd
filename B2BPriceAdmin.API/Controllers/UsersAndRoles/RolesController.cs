using B2BPriceAdmin.Core.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B2BPriceAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        public RolesController()
        {

        }

        [HttpGet("GetAllRoles")]
        [HasPermission(Permissions.RoleRead)]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok();
        }
    }
}