using B2BPriceAdmin.Core.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B2BPriceAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        public PermissionsController()
        {

        }

        [HttpGet("GetAllRolePermissions")]
        [HasPermission(Permissions.RoleRead)]
        public async Task<IActionResult> GetAllRolePermissions()
        {
            return Ok();
        }
    }

}