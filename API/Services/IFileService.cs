using System.Collections.Generic;
using Application.Models.Tickets;
using Microsoft.AspNetCore.Http;

namespace API.Services
{
    public interface IFileService
    {
        void UploadFile(List<IFormFile> files);  
        (string fileType, byte[] archiveData, string archiveName) DownloadFiles();  
        string SizeConverter(long bytes); 
    }
}