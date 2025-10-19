using Microsoft.EntityFrameworkCore;
using ProjectWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.seller_request
{
    public class SellerRequestTypeRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<SellerRequestType, long>(contextFactory)
    {
        
    }
}
