using DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Payed { get; set; }

        public int UserId { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public Status Status { get; set; }

        [ForeignKey("UserId")]
        public User user  { get; set; }

        public ICollection<CartItems> CartItems { get; set; }
    }
}
