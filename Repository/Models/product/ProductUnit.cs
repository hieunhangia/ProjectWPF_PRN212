using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    [Table("product_unit")]
    public class ProductUnit
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name", TypeName = "nvarchar(100)")]
        public required string Name { get; set; }

        // Navigation property - one-to-many relationship with Products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}