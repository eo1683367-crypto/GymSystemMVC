using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class AttachementServices : IAttachementServices
    {
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5 mb

        private readonly string[] allowedExtentions = {".jpg", ".jpeg",".png"};
        private readonly ILogger<AttachementServices> logger;
        private readonly IWebHostEnvironment env;

        public AttachementServices(ILogger<AttachementServices> logger, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }
        public async Task<string?> UploudAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead) return null;

            if (fileStream.Length == 0) return null;

            if(fileStream.Length > maxFileSize)
            {
                logger.LogWarning("Rejected File Too Large!");
                return null;
            }
            //

            var extention = Path.GetExtension(fileName);

            if(string.IsNullOrEmpty(extention) || !allowedExtentions.Contains(extention))
            {
                logger.LogWarning("Rejected Wrong Extention File!");
                return null;
            }

            var uploudedFolder = Path.Combine(env.ContentRootPath, folderName);

            Directory.CreateDirectory(uploudedFolder);

            var storedFileName = $"{Guid.NewGuid()}{extention}";

            var filePath = Path.Combine(uploudedFolder,storedFileName);

            try
            {
                await using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);

                await fileStream.CopyToAsync(fs);
                return storedFileName;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed To Upload File");
                return null;
            }
        }
        public bool Delete(string fileName, string FolderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(FolderName)) return false;

            try
            {
                var filePath = Path.Combine(env.ContentRootPath,FolderName ,fileName);
                if(!File.Exists(filePath)) return false;

                File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Failed To Delete Attachement");
                return false;
            }
            
        }

        public (Stream stream, string contentType)? GetFile(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName) || string.IsNullOrWhiteSpace(fileName)) return null;

            var filePath = Path.Combine(env.ContentRootPath, folderName, fileName);

            if (!File.Exists(filePath)) return null;

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var extension = Path.GetExtension(filePath).ToLower();

            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream" // Binary Data
            };

            return (stream, contentType);
        }
    }
}
