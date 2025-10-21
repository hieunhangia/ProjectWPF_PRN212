using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    [Table("product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }
        [Column("name", TypeName = "nvarchar(255)")]
        public required string Name { get; set; }
        [Column("description", TypeName = "nvarchar(255)")]
        public required string Description { get; set; }
        [Column("price")]
        public int Price { get; set; }
        [Column("enabled")]
        public bool IsActive { get; set; } = true;

        [Column("product_unit_id")]
        public long ProductUnitId { get; set; }

        [ForeignKey("ProductUnitId")]
        public virtual ProductUnit ProductUnit { get; set; } = null!;

        // Navigation property - one-to-many relationship with ProductBatches
        public virtual ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
    }
}
