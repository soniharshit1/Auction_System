using Auction_System_Library_Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class PersonProfileDTO
    {
        // Person Information
        public string Name { get; set; } = null!;

        public int PersonId { get; set; } // <--- ADD THIS PROPERTY
        public string Email { get; set; } = null!;
        public string? ContactNumber { get; set; }

        // Ensure you include the Role for frontend logic
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }

        // --- Combined Data ---
        // The list of reviews where this person is the seller.
        public IEnumerable<ReviewDTO> SellerReviews { get; set; } = new List<ReviewDTO>();

  
    }
}
