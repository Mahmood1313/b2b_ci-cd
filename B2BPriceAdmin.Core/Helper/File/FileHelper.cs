using AutoMapper;
using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.DTO.Base;
using Microsoft.AspNetCore.Http;

namespace B2BPriceAdmin.Core.Helper
{
    public class FileHelper : IFileHelper
    {
        private readonly IFileHandler _fileHandler;
        private readonly IMapper _mapper;

        public FileHelper(IFileHandler fileHandler, IMapper mapper)
        {
            this._fileHandler = fileHandler;
            this._mapper = mapper;
        }
        public async Task<T> UploadSingleAttachment<T>(IFormFile attachment, string relativePath, bool S3)
        {
            return (_mapper.Map<T>(await _fileHandler.UploadSingleFile(attachment, relativePath, S3)));
        }
        public async Task<List<T>> UploadAllAttachments<T>(IFormFileCollection attachments, string relativePath, bool S3)
        {
            return (_mapper.Map<List<T>>(await _fileHandler.UploadAllFiles(attachments, relativePath, S3)));
        }
        public void DeleteSingleAttachment(string relativePath, bool S3)
        {
            _fileHandler.DeleteSingleFile(relativePath, S3);
        }
        public void DeleteAllAttachments<T>(List<T> uploadedFiles, bool S3) where T : BaseAttachmentDTO
        {
            var paths = uploadedFiles.Select(x => x.Path).ToList();
            _fileHandler.DeleteAllFile(paths, S3);
        }
        public void DeleteAttachmentsWhereTrue<T>(List<T> uploadedFiles, bool S3) where T : BaseAttachmentDTO
        {
            var paths = uploadedFiles.Where(x => x.Deleted == true).Select(x => x.Path).ToList();
            _fileHandler.DeleteAllFile(paths, S3);
        }
        public void DeleteAttachmentsWhereIdZero<T>(List<T> uploadedFiles, bool S3) where T : BaseAttachmentDTO
        {
            var paths = uploadedFiles.Where(x => x.Id == 0).Select(x => x.Path).ToList();
            _fileHandler.DeleteAllFile(paths, S3);
        }

    }
}
