using Microsoft.AspNetCore.Http;

namespace API.Models
{
    public class FilesModel
    {
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}