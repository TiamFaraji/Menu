﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {
        private readonly string _storagePath;
        public FileUploadService(IConfiguration configuration)
        {
            _storagePath = configuration["FileUpload:StoragePath"];
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            if(file==null || file.Length == 0)
            {
                throw new ArgumentException("file is empty or not provided");
            }
             
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullpath = Path.Combine(_storagePath, filename);

            using (var stream = new FileStream(fullpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filename;
        }
    }
}
