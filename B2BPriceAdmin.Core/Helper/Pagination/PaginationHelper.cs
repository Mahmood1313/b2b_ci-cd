using B2BPriceAdmin.Core.Common.PagedResponse;
using B2BPriceAdmin.DTO;

namespace B2BPriceAdmin.Core.Helper.Pagination
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilterInputDTO filter, int totalRecords)
        {
            var respose = new PagedResponse<List<T>>(pagedData, filter.PageNumber, filter.PageSize);
            int roundedTotalPages = 0;
            if (totalRecords > 0)
            {
                var totalPages = totalRecords / (double)filter.PageSize;
                roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            }
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}
