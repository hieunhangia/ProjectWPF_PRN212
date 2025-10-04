using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.user
{
    [Table("role")]
    public class Role
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name", TypeName = "varchar(55)")]
        public required string Name { get; set; }

        public virtual User? User { get; set; }
    }
}