using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.user
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("email", TypeName = "nvarchar(255)")]
        public required string Email { get; set; }

        [Column("password", TypeName = "nvarchar(255)")]
        public string? Password { get; set; }

        [Column("enabled")]
        public bool IsActive { get; set; } = false;

        // Foreign key for one-to-one relationship with Role
        [Column("role_id")]
        public long RoleId { get; set; }

        // Navigation property - one-to-one relationship with Role
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;
    }
}