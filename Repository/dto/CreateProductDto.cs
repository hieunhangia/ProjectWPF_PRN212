namespace Repository.dto
{
    public class CreateProductDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public bool IsActive { get; set; } = true;
        public ProductUnit ProductUnit { get; set; } = null!;
    }
}
