using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B2BPriceAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class BaseController<CDTO, UDTO, GADTO, GIDTO> : Controller where CDTO : BaseDTO where UDTO : BaseDTO where GADTO : BaseDTO where GIDTO : BaseDTO
    {
        private IBaseService<CDTO, UDTO, GADTO, GIDTO> _baseService;
        protected BaseController(IBaseService<CDTO, UDTO, GADTO, GIDTO> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("GetAll")]
        public virtual async Task<IActionResult> GetAll([FromQuery] PaginationFilterInputDTO filter)
        {
            return Ok(await _baseService.GetAllAsync(filter));
        }

        [HttpGet("GetById")]
        public virtual async Task<IActionResult> GetById([FromQuery] BaseDTO Dto)
        {
            var result = await _baseService.GetByIdAsync(Dto);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("AddUpdate")]
        [ValidateModel]
        public virtual async Task<IActionResult> AddUpdate(CDTO Dto)
        {
            if (Dto.Id == 0 || Dto.Id == null)
            {
                return Ok(await _baseService.CreateAsync(Dto));
            }
            else
            {
                var result = await _baseService.UpdateAsync(Dto.Id ?? 0, Dto);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
        }

        [HttpPost("AddUpdateList")]
        [ValidateModel]
        public virtual async Task<IActionResult> AddUpdate(List<CDTO> Dto)
        {
            return Ok(await _baseService.CreateUpdateAsync(Dto));
        }

        [HttpDelete("DeleteById")]
        public virtual async Task<IActionResult> DeleteById([FromQuery] BaseDTO Dto)
        {
            return Ok(await _baseService.DeleteAsync(Dto));
        }
    }

}
