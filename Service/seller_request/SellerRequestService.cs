using ProjectWPF.Models;
using Repository.Models.user;
using Repository.Repository.seller_request;
using Service.user;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Service.seller_request
{
    public class SellerRequestService(SellerRequestRepository sellerRequestRepository,
        SellerRequestStatusService sellerRequestStatusService,
        SellerRequestTypeService sellerRequestTypeService,
        SellerService sellerService)
    {
        private readonly SellerRequestRepository _sellerRequestRepository = sellerRequestRepository;
        private readonly SellerRequestStatusService _sellerRequestStatusService = sellerRequestStatusService;
        private readonly SellerRequestTypeService _sellerRequestTypeService = sellerRequestTypeService;
        private readonly SellerService _sellerService = sellerService;


        public List<SellerRequest> getAllSellerRequest()
        {
            return _sellerRequestRepository.GetAll();
        }

        public SellerRequest? getSellerRequestById(long id)
        {
            return _sellerRequestRepository.GetById(id);
        }

        public void SaveAddRequest<T>(T entity, Seller seller)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var sellerRequest = new SellerRequest()
            {
                Content = JsonSerializer.Serialize(entity,options),
                SellerId = seller.Id,
                RequestTypeId = _sellerRequestTypeService.GetAddType()!.Id,
                StatusId = _sellerRequestStatusService.GetPendingStatus()!.Id,
                CreatedAt = DateTime.Now
            };
            _sellerRequestRepository.Add(sellerRequest);
        }

        public void SaveUpdateRequest<T>(T entity,long oldEntityId, Seller seller)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping           
            };
            var sellerRequest = new SellerRequest()
            {
                Content = JsonSerializer.Serialize(entity,options),
                Seller = seller,
                RequestType = _sellerRequestTypeService.GetUpdateType()!,
                Status = _sellerRequestStatusService.GetPendingStatus()!,
                CreatedAt = DateTime.Now,
                OldContentId = oldEntityId
            };
            _sellerRequestRepository.Add(sellerRequest);
        }

        public void approveRequest<T>(long requestId,Action<T> addMethod, Action<T> updateMethod)
        {
            SellerRequest? sellerRequest = _sellerRequestRepository.GetById(requestId);
            if (sellerRequest == null) {
                throw new Exception("Không tìm thây sellerRequest");
            }
            sellerRequest.Status = _sellerRequestStatusService.GetApprovedStatus()!;
            string requestTypeName = sellerRequest.RequestType.Name;
            if (_sellerRequestTypeService.IsAddType(sellerRequest))
            {
                T? entity = JsonSerializer.Deserialize<T>(sellerRequest.Content);
                addMethod.Invoke(entity);
            }
            else if (_sellerRequestTypeService.IsUpdateType(sellerRequest)) {
                T? entity = JsonSerializer.Deserialize<T>(sellerRequest.Content);
                updateMethod.Invoke(entity);
            }
            _sellerRequestRepository.Update(sellerRequest);
        }

        public void RejectRequest(long requestId) {
            SellerRequest? sellerRequest = _sellerRequestRepository.GetById(requestId);
            if (sellerRequest == null)
            {
                throw new Exception("Không tìm thây sellerRequest");
            }
            sellerRequest.Status = _sellerRequestStatusService.GetRejectedStatus()!;
            _sellerRequestRepository.Update(sellerRequest);
        }
    }
}
