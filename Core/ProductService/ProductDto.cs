using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductService
{
    public class ProductDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required int Price { get; set; }
        public string? Description { get; set; }
        public IFormFile?Img { get; set; }
        public string?ImgName { get; set; }
        public string? Combinations { get; set; }
        public int CategoryId { get; set; }
    }
}
