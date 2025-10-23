using Repository.Models.user;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWPF.Models;


[Table("seller_request")]
public partial class SellerRequest
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "BIGINT")]
    public long Id { get; set; }

    [Required]
    [Column(TypeName = "DATETIME")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Column(TypeName = "BIGINT")]
    public long RequestTypeId { get; set; }


    [Required]
    [Column(TypeName = "BIGINT")]
    public long SellerId { get; set; }

    [Required]
    [Column(TypeName = "BIGINT")]
    public long StatusId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Content { get; set; } = null!;
    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string EntityName { get; set; } = null!;


    [Column(TypeName = "nvarchar(max)")]
    public string? OldContent { get; set; } = null!;

    public virtual SellerRequestType RequestType { get; set; } = null!;

    public virtual Seller Seller { get; set; } = null!;

    public virtual SellerRequestStatus Status { get; set; } = null!;
}
