using Repository.Models.user;
using Repository.Repository.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.user
{
    public class SellerService(SellerRepository sellerRepository)
    {
        private readonly SellerRepository _sellerRepository = sellerRepository;

        public List<Seller> GetAllSellers() => _sellerRepository.GetAll();

        public Seller? GetSellerById(long id) => _sellerRepository.GetById(id);

        public List<Seller> GetSellersByCondition(Expression<Func<Seller, bool>> condition) => _sellerRepository.GetByCondition(condition);

        public bool IsEmailExists(string email) => _sellerRepository.GetByCondition(s => s.Email == email).FirstOrDefault() != null;

        public bool IsIdentifyExists(string identify) => _sellerRepository.GetByCondition(s => s.Cid == identify).FirstOrDefault() != null;
        public bool IsIdentifyExists(string identify, long excludeSellerId)
        {
            return _sellerRepository.GetByCondition(s => s.Cid == identify && s.Id != excludeSellerId).FirstOrDefault() != null;
        }

        public void AddSeller(Seller seller) => _sellerRepository.Add(seller);

        public void UpdateSeller(Seller seller) => _sellerRepository.Update(seller);

        public void DeleteSeller(long sellerId) => _sellerRepository.Delete(sellerId);
    }
}
