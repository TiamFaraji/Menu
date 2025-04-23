using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class CartItems
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public DateTime Created { get; set; }

        [ForeignKey("CartId")]
        public Cart cart { get; set; }

        [ForeignKey("ProductId")]
        public Product product { get; set; }
    }
}
