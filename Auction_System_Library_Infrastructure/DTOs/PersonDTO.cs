using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD:Auction_System_Library_Infrastructure/DTOs/PersonDTO.cs
namespace Auction_System_Library_Infrastructure.DTOs
=======
namespace Auction_System_Library.DTOs
>>>>>>> c1a446949e1028e1cc626573859ba5736da3f86f:Auction_System_Library/DTOs/PersonDTO.cs
{
    public class PersonDTO
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string? ContactNumber { get; set; }
    }
}
