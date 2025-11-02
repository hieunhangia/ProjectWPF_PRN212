using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Repository.Models.user
{
    [Table("user_token")]
    public class UserToken
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("token", TypeName = "varchar(255)")]
        public required string Token { get; set; }

        [Column("expiration")]
        [JsonIgnore]
        public DateTime Expiration { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User? User { get; set; }
    }
}
