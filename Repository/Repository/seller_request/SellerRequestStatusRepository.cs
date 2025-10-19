using Microsoft.EntityFrameworkCore;
using ProjectWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.seller_request
{
    public class SellerRequestStatusRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<SellerRequestStatus, long>(contextFactory)
    {
        
    }
}
