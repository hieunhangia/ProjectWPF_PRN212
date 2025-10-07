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

        public bool IsEmailExists(string email)
        {
            return _sellerRepository.GetByCondition(s => s.Email == email).FirstOrDefault() != null;
        }

        public bool IsIdentifyExists(string identify)
        {
            return _sellerRepository.GetByCondition(s => s.Cid == identify).FirstOrDefault() != null;
        }

        public void AddSeller(Seller seller)
        {
            _sellerRepository.Add(seller);
        }
    }
}
