using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.DataAccess.Postgres.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int Amount { get; set; }
        public string EmailClient { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
