using B2BPriceAdmin.DTO.Base;
using Microsoft.AspNetCore.Http;

namespace B2BPriceAdmin.Core.Interfaces
{
    public interface IFileHelper
    {
        public Task<T> UploadSingleAttachment<T>(IFormFile attachment, string relativePath, bool S3);
        public Task<List<T>> UploadAllAttachments<T>(IFormFileCollection assetAttachments, string relativePath, bool S3);
        public void DeleteSingleAttachment(string relativePath, bool S3);
        public void DeleteAllAttachments<T>(List<T> uploadedFiles, bool S3) where T : BaseAttachmentDTO;
        public void DeleteAttachmentsWhereTrue<T>(List<T> uploadedFiles, bool S3) where T : BaseAttachmentDTO;
        public void DeleteAttachmentsWhereIdZero<T>(List<T> uploadedFiles, bool S3) where T : BaseAttachmentDTO;
    }
}
