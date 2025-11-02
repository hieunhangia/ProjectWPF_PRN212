namespace Repository.dto
{
    public class ProductBatchDto
    {
        public long Id { get; set; }
        public string? ExpiryDate { get; set; }
        public string? Quantity { get; set; }
        public long ProductId { get; set; }
    }
}