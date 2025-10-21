using Microsoft.EntityFrameworkCore;
using ProjectWPF.Models;

namespace Repository.Repository.seller_request
{
    public class SellerRequestTypeRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<SellerRequestType, long>(contextFactory)
    {

    }
}
