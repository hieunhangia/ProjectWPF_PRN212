using Microsoft.EntityFrameworkCore;
using ProjectWPF.Models;

namespace Repository.Repository.seller_request
{
    public class SellerRequestStatusRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<SellerRequestStatus, long>(contextFactory)
    {

    }
}
