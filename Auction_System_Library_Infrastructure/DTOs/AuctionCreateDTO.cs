namespace Auction_System_Library_Infrastructure.DTOs
{
    public class AuctionCreateDTO
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public decimal StartPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<AddAuctionProductAttributesDTO> Attributes { get; set; } = new();
    }
}