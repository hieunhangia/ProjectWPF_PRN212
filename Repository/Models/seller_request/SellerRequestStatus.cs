using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWPF.Models;

[Table("seller_request_status")]
[Index(nameof(Name), IsUnique = true)]
public partial class SellerRequestStatus
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "BIGINT")]
    public long Id { get; set; }
    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string Name { get; set; } = null!;

    public virtual ICollection<SellerRequest> SellerRequests { get; set; } = new List<SellerRequest>();
}
