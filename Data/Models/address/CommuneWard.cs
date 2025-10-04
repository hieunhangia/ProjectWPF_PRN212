using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.address
{
    [Table("commune_ward")]
    public class CommuneWard
    {
        [Key]
        [Column("code", TypeName = "nvarchar(255)")]
        public required string Code { get; set; }
        [Column("name",TypeName ="nvarchar(100)")]
        public required string Name { get; set; } 

        [ForeignKey("ProvinceCity")]
        public required string ProvinceCode { get; set; } 

        public virtual ProvinceCity ProvinceCity { get; set; } = null!;
    }
}