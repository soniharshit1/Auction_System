using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Auction_System_Library_Database.Enums;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class PersonDTO
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; } 

        public string? ContactNumber { get; set; }
    }
}
