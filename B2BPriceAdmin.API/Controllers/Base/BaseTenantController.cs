using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.DTO;

namespace B2BPriceAdmin.API.Controllers
{

    public abstract class BaseTenantController<CDTO, UDTO, GADTO, GIDTO> : BaseController<CDTO, UDTO, GADTO, GIDTO>
        where CDTO : BaseDTO where UDTO : BaseDTO where GADTO : BaseDTO where GIDTO : BaseDTO
    {
        private readonly IBaseTenantService<CDTO, UDTO, GADTO, GIDTO> _baseService;
        protected BaseTenantController(IBaseTenantService<CDTO, UDTO, GADTO, GIDTO> baseService) : base(baseService)
        {
            _baseService = baseService;
        }
    }
}
