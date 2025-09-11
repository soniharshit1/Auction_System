using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library.DTOs
{
    public class ProductDTO
    {
        public string ProductName { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
