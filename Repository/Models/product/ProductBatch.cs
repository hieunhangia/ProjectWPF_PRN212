using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    [Table("product_batch")]
    public class ProductBatch
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        // Foreign key for many-to-one relationship with Product
        [Column("product_id")]
        public long ProductId { get; set; }

        // Navigation property - many-to-one relationship with Product
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}