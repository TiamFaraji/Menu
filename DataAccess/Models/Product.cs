using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string Title {  get; set; }
        public required int Price {  get; set; }
        public string? Description { get; set; }
        public string? Img {  get; set; }
        public string? Combinations {  get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public ICollection<CartItems> CartItems { get; set; }
    }
}
