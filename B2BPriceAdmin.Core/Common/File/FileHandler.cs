using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using B2BPriceAdmin.Common.Extensions;
using B2BPriceAdmin.Core.Exceptions;
using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace B2BPriceAdmin.Core.Common.File
{
    public class FileHandler : IFileHandler
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private string BUCKET_NAME = "";
        private string AWS_ACCESS_KEY_ID = "";
        private string AWS_SECRET_ACCESS_KEY = "";
        private string FOLDER_NAME = "";
        private readonly AmazonS3Client _amazonS3Client;
        public FileHandler(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

            BUCKET_NAME = _configuration["S3:BucketName"];
            AWS_ACCESS_KEY_ID = _configuration["S3:AWSAccessKeyId"];
            AWS_SECRET_ACCESS_KEY = _configuration["S3:AWSSecretAccessKey"];
            FOLDER_NAME = _configuration["S3:FolderName"];
            _amazonS3Client = new AmazonS3Client(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, RegionEndpoint.MESouth1);
        }

        public async Task<FileDTO> UploadSingleFile(IFormFile file, string relativePath, bool S3)
        {
            var fileDTO = new FileDTO();
            string GUID = Guid.NewGuid().ToString();
            string uniqueFileName = GUID + "_" + StringExtensions.RemoveExcept(file.FileName);
            string originalFileName = file.FileName;
            string fileContentType = file.ContentType;
            long fileLength = file.Length;
            string path = S3 ? $"{FOLDER_NAME}{relativePath}{uniqueFileName}" : $"{relativePath}{uniqueFileName}";
            try
            {
                if (file.Length > 0)
                {
                    if (!S3)
                    {

                        var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath + relativePath);
                        Directory.CreateDirectory(uploadFolder);
                        string filepath = Path.Combine(uploadFolder, uniqueFileName);
                        using (FileStream fs = new FileStream(filepath, FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }

                        fileDTO.OriginalFileName = originalFileName;
                        fileDTO.SavedFileName = uniqueFileName;
                        fileDTO.FileContentType = fileContentType;
                        fileDTO.FileLength = fileLength;
                        fileDTO.FileGUID = GUID;
                        fileDTO.Path = path;
                    }
                    else
                    {
                        //Multipart Upload

                        var fileTransferUtility = new TransferUtility(_amazonS3Client);
                        using (Stream fileToUpload = file.OpenReadStream())
                        {
                            await fileTransferUtility.UploadAsync(fileToUpload, BUCKET_NAME, path);
                        }

                        //Simple Upload

                        //using (Stream fileToUpload = file.OpenReadStream())
                        //{
                        //    var putObjectRequest = new PutObjectRequest();
                        //    putObjectRequest.BucketName = BUCKET_NAME;
                        //    putObjectRequest.Key = path;
                        //    putObjectRequest.InputStream = fileToUpload;
                        //    putObjectRequest.ContentType = file.ContentType;

                        //    var response = await _amazonS3Client.PutObjectAsync(putObjectRequest);

                        //    if (!(response.HttpStatusCode == System.Net.HttpStatusCode.OK))
                        //    {
                        //        throw new UserFriendlyException("Unable to upload file" );
                        //    }
                        //}

                        fileDTO.OriginalFileName = originalFileName;
                        fileDTO.SavedFileName = uniqueFileName;
                        fileDTO.FileContentType = fileContentType;
                        fileDTO.FileLength = fileLength;
                        fileDTO.FileGUID = GUID;
                        fileDTO.Path = path;
                    }
                }
                return fileDTO;
            }
            catch (AmazonS3Exception e)
            {
                DeleteSingleFile(path, S3); // if exception occer before setting FilePathOnServer then file can not be deleted
                throw new UserFriendlyException("Unable to upload file");
            }

            catch (Exception)
            {
                DeleteSingleFile(path, S3); // if exception occer before setting FilePathOnServer then file can not be deleted
                throw;
            }

        }

        public async Task<List<FileDTO>> UploadAllFiles(IFormFileCollection uploadCollection, string relativePath, bool S3) // relativePath After wwwroot like /CompanyDocs/{compID}/AssetImage/ must ended on /
        {
            var fileDTOs = new List<FileDTO>();
            try
            {
                foreach (var file in uploadCollection)
                {
                    // Saving file on Server
                    if (file.Length > 0)
                    {
                        fileDTOs.Add(await UploadSingleFile(file, relativePath, S3));
                    }
                }
                return fileDTOs;
            }
            catch (Exception)
            {
                foreach (var item in fileDTOs)
                {
                    DeleteSingleFile(item.Path, S3);
                }
                throw;
            }
        }
        public void DeleteFolder(string relativePath)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath + relativePath);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
        public void DeleteSingleFile(string relativePath, bool S3)
        {
            if (!S3)
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath + relativePath);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            else
            {
                var result = _amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = BUCKET_NAME, Key = relativePath });
            }

        }

        public void DeleteAllFile(List<string> relativePaths, bool S3)
        {
            foreach (var path in relativePaths)
            {
                DeleteSingleFile(path, S3);
            }
        }

    }
}
