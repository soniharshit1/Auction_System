using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class UpdatedPersonDTO
    {
      
            public string Name { get; set; } = null!;

            public string Email { get; set; } = null!;

            public string? ContactNumber { get; set; }
        }
    }

