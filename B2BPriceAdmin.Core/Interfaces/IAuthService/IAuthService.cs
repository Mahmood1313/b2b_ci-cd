using B2BPriceAdmin.Core.Common.PagedResponse;
using B2BPriceAdmin.DTO;

namespace B2BPriceAdmin.Core.Interfaces
{
    public interface IAuthService
    {
        public Task<Response<AuthResponseDTO>> Authenticate(LoginDTO loginCredentials);
    }
}
