using B2BPriceAdmin.Core.Common.PagedResponse;
using B2BPriceAdmin.DTO;

namespace B2BPriceAdmin.Core.Interfaces
{
    public interface IBaseService<CDTO, UDTO, GADTO, GIDTO>
    {
        public Task<PagedResponse<List<GADTO>>> GetAllAsync(PaginationFilterInputDTO filter);
        public Task<Response<GIDTO>> GetByIdAsync(BaseDTO Dto);
        public Task<Response<CDTO>> CreateAsync(CDTO Dto);
        public Task<Response<List<CDTO>>> CreateUpdateAsync(List<CDTO> Dto);
        public Task<Response<UDTO>> UpdateAsync(int Id, CDTO Dto);
        public Task<Response<bool>> DeleteAsync(BaseDTO Dto);
    }
}
