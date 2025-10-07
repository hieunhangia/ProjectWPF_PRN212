using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository
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

        [NotMapped]
        public string Status
        { 
            get => IsActive ? "Đang Mở Khoá" : "Đã Khoá";
        }
    }
}