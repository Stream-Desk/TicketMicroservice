using System;
using System.Collections.Generic;
using Domain.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Files
{
    public class FileRequest
    {
        public List<IFormFile> Files { get; set; } 
        public string BaseUrl { get; set; }
        
    }
}