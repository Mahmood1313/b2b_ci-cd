namespace B2BPriceAdmin.DTO
{
    public class FileDTO
    {
        public string OriginalFileName { get; set; }
        public string SavedFileName { get; set; }
        public string FileContentType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }
        public string Path { get; set; }

    }
}
