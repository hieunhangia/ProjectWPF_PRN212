using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models.user
{
    public class Seller : User
    {
        [Column("full_name", TypeName = "nvarchar(255)")]
        public required string FullName { get; set; } 
        [Column("birth_date", TypeName = "date")]
        public DateOnly BirthDate { get; set; }
        [Column("cid", TypeName = "nvarchar(50)")]
        public required string Cid { get; set; }
        [Column("specific_address", TypeName = "nvarchar(200)")]
        public required string SpecificAddress { get; set; }


        [Column("commune_ward_code",TypeName ="nvarchar(255)")]
        public required string CommuneWardCode { get; set; }

        [ForeignKey("CommuneWardCode")]
        public virtual CommuneWard? CommuneWard { get; set; }

        [NotMapped]
        public string Address
        {
            get => $"{SpecificAddress}, {CommuneWard?.Name}, {CommuneWard?.ProvinceCity.Name}";
        }

    }
}
