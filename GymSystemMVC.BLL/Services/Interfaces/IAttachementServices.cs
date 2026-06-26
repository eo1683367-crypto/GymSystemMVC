using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemMVC.BLL.Services.Interfaces
{
    public interface IAttachementServices
    {
         Task<string?> UploudAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default);

         bool Delete(string fileName, string FolderName);

        (Stream stream , string contentType)? GetFile(string fileName, string FolderName);


    }
}
