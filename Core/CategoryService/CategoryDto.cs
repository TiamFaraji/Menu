using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CategoryService
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public IFormFile? Img { get; set; }
    }
}
