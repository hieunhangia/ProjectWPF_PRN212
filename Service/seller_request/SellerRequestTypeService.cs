using ProjectWPF.Models;
using Repository.Repository.seller_request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.seller_request
{
    public class SellerRequestTypeService(SellerRequestTypeRepository sellerRequestTypeRepository)
    {

        private readonly SellerRequestTypeRepository _sellerRequestTypeRepository = sellerRequestTypeRepository;
        public SellerRequestType? GetAddType()
        {
            return _sellerRequestTypeRepository.GetByCondition(s => s.Name == "Thêm mới").FirstOrDefault();
        }

        public SellerRequestType? GetUpdateType()
        {
            return _sellerRequestTypeRepository.GetByCondition(s => s.Name == "Cập nhật").FirstOrDefault();
        }

        public bool IsAddType(SellerRequest request)
        {
            return request.RequestType.Name == "Thêm mới";
        }

        public bool IsUpdateType(SellerRequest request)
        {
            return request.RequestType.Name == "Cập nhật";
        }
    }
}
