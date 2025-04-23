using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Category
    {
        [Key]
        public int Id {  get; set; }
        public required string Title {  get; set; }

        public string? Img { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
