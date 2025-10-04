using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.product
{
    [Table("product_unit")]
    public class ProductUnit
    {
        [Key]
        public long Id { get; set; }

        [Column("name", TypeName = "nvarchar(100)")]
        public required string Name { get; set; }

        // Navigation property - one-to-many relationship with Products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}