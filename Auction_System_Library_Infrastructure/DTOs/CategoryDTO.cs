using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class CategoryDTO
    {
        public string CategoryName { get; set; } = null!;

        public bool? IsActive { get; set; }
    }
}
