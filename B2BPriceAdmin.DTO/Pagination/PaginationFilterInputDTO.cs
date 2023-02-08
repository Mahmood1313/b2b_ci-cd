namespace B2BPriceAdmin.DTO
{
    public class PaginationFilterInputDTO
    {
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; }
        public string SortColumn { get; set; } = "Id";
        public string SortOrder { get; set; }
        public string Search { get; set; }
    }
}
