namespace B2BPriceAdmin.DTO.Base
{
    public class BaseAttachmentDTO : BaseDTO
    {
        //public int Id { get; set; }
        public string OriginalFileName { get; set; }
        public string SavedFileName { get; set; }
        public string FileContentType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }
        public string Path { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
