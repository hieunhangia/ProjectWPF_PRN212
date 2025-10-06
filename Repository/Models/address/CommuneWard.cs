using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    [Table("commune_ward")]
    public class CommuneWard
    {
        [Key]
        [Column("code", TypeName = "nvarchar(255)")]
        public required string Code { get; set; }
        [Column("name",TypeName ="nvarchar(100)")]
        public required string Name { get; set; } 

        [Column("province_city_code")]
        public required string ProvinceCityCode { get; set; }

        [ForeignKey("ProvinceCityCode")]
        public virtual ProvinceCity ProvinceCity { get; set; } = null!;
    }
}