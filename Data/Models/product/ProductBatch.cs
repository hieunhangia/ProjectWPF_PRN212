using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.product
{
    [Table("product_batch")]
    public class ProductBatch
    {
        [Key] // Add Key attribute
        public long Id { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }

        // Foreign key for many-to-one relationship with Product
        [Column("product_id")]
        public long ProductId { get; set; }

        // Navigation property - many-to-one relationship with Product
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}