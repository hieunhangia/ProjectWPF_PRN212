using System;
using System.Collections.Generic;

namespace ProjectWPF.Models;

public partial class SellerRequestStatus
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SellerRequest> SellerRequests { get; set; } = new List<SellerRequest>();
}
