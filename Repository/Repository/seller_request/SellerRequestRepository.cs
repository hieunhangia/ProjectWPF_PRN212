using Microsoft.EntityFrameworkCore;
using ProjectWPF.Models;

namespace Repository.Repository.seller_request
{
    public class SellerRequestRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<SellerRequest, long>(contextFactory)
    {
        public IEnumerable<SellerRequest> GetAllSellerRequest()
        {
            using var context = _contextFactory.CreateDbContext();
            return context.SellerRequests
                .Include(r => r.Seller)
                .Include(r => r.Status)
                .Include(r => r.RequestType)
                .ToList();
        }

        public SellerRequest? getSellerRequestById(long id)
        {
            using var context = _contextFactory.CreateDbContext();
            return context.SellerRequests
                .Include(r => r.Seller)
                .Include(r => r.Status)
                .Include(r => r.RequestType)
                .FirstOrDefault(r => r.Id == id);
        }
    }
}
