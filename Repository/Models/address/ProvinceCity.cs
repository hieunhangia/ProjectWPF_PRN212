using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
{
    [Table("province_city")]
    public class ProvinceCity
    {
        [Key]
        public required string Code { get; set; }
        [Column("name", TypeName = "nvarchar(255)")]
        public required string Name { get; set; } 

        public virtual ICollection<CommuneWard> CommuneWards { get; set; } = new List<CommuneWard>();
    }
}