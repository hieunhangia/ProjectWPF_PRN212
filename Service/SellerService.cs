using Repository;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SellerService
    {
        private readonly SellerRepository _sellerRepository = new();

        public List<Seller> GetAllSellers()
        {
            return _sellerRepository.GetAll();
        }
    }
}
