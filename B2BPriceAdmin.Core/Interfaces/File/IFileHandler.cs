using B2BPriceAdmin.DTO;
using Microsoft.AspNetCore.Http;

namespace B2BPriceAdmin.Core.Interfaces
{
    public interface IFileHandler
    {
        public Task<FileDTO> UploadSingleFile(IFormFile file, string relativePath, bool S3);
        public Task<List<FileDTO>> UploadAllFiles(IFormFileCollection uploadCollection, string relativePath, bool S3);
        public void DeleteFolder(string relativePath);
        public void DeleteSingleFile(string relativePath, bool S3);
        public void DeleteAllFile(List<string> relativePaths, bool S3);
    }
}
