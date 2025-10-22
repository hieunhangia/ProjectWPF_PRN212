using Microsoft.Identity.Client;
using ProjectWPF.Models;
using Repository.Repository.seller_request;

namespace Service.seller_request
{
    public class SellerRequestTypeService(SellerRequestTypeRepository sellerRequestTypeRepository)
    {

        private readonly SellerRequestTypeRepository _sellerRequestTypeRepository = sellerRequestTypeRepository;

        public SellerRequestType? GetUpdateType()
        {
            return _sellerRequestTypeRepository.GetByCondition(s => s.Name == "Cập nhật").FirstOrDefault();
        }

        public SellerRequestType? GetAddType()
        {
            return _sellerRequestTypeRepository.GetByCondition(s => s.Name == "Thêm mới").FirstOrDefault();
        }
        public SellerRequestType? GetById(long id)
        {
            return _sellerRequestTypeRepository.GetById(id);
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
