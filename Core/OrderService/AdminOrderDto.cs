using DataAccess.Enums;

namespace Admin.Models
{
    public class AdminOrderDto
    {
        public int Id { get; set; }
        public DateTime Payed { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Status Status { get; set; }
        public string UserName { get; set; }
        public List<AdminOrderItemDto> Items { get; set; } = new List<AdminOrderItemDto>();
    }
}
