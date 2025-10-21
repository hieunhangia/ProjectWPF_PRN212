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
    }
}