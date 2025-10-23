namespace Auction_System_Library_Infrastructure.DTOs
{
    public class AddAuctionProductAttributesDTO
    {
        public int AttributeId { get; set; }
        public string AttributeValue { get; set; } = string.Empty;
    }
}