using System.Collections.Generic;

namespace Repository.dto
{
    public class ProductWithBatchesDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public bool IsActive { get; set; } = true;
        public ProductUnit ProductUnit { get; set; } = null!;
        public List<ProductBatchDto> ProductBatches { get; set; } = new();
    }
}