using Repository.Models.user;
using System;
using System.Collections.Generic;

namespace ProjectWPF.Models;

public partial class SellerRequest
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public long RequestTypeId { get; set; }

    public long SellerId { get; set; }

    public long StatusId { get; set; }

    public string Content { get; set; } = null!;

    public string? EntityName { get; set; }

    public string OldContent { get; set; } = null!;

    public virtual SellerRequestType RequestType { get; set; } = null!;

    public virtual Seller Seller { get; set; } = null!;

    public virtual SellerRequestStatus Status { get; set; } = null!;
}
