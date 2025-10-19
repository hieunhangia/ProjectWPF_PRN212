using Microsoft.EntityFrameworkCore;
using ProjectWPF.Models;
using Repository.Models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.seller_request
{
    public class SellerRequestRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<SellerRequest, long>(contextFactory)
    {

    }
}
