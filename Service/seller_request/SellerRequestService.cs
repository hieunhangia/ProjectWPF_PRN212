using Microsoft.EntityFrameworkCore;
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


        public IEnumerable<SellerRequest> getSellerRequestsContext()
        {
            return _sellerRequestRepository.GetAllSellerRequest();
        }



        public SellerRequest? getSellerRequestById(long id)
        {
            return _sellerRequestRepository.getSellerRequestById(id);
        }

        public void SaveAddRequest<T>(T entity, Seller seller)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            var sellerRequest = new SellerRequest()
            {
                Content = JsonSerializer.Serialize(entity, options),
                SellerId = seller.Id,
                RequestTypeId = _sellerRequestTypeService.GetAddType()!.Id,
                StatusId = _sellerRequestStatusService.GetPendingStatus()!.Id,
                CreatedAt = DateTime.Now,
                EntityName = typeof(T).Name
            };
            _sellerRequestRepository.Add(sellerRequest);
        }

        public void SaveUpdateRequest<T>(T entity,T oldEntity,Seller seller)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            var sellerRequest = new SellerRequest()
            {
                Content = JsonSerializer.Serialize(entity, options),
                SellerId = seller.Id,
                RequestTypeId = _sellerRequestTypeService.GetUpdateType()!.Id,
                StatusId = _sellerRequestStatusService.GetPendingStatus()!.Id!,
                CreatedAt = DateTime.Now,
                OldContent = JsonSerializer.Serialize(oldEntity, options),
                EntityName = typeof(T).Name
            };
            _sellerRequestRepository.Add(sellerRequest);
        }

        public void approveRequest<T>(long requestId, Action<T> addMethod, Action<T> updateMethod)
        {
            SellerRequest? sellerRequest = _sellerRequestRepository.getSellerRequestById(requestId);
            if (sellerRequest == null)
            {
                throw new Exception("Không tìm thây sellerRequest");
            }
            sellerRequest.Status = _sellerRequestStatusService.GetApprovedStatus()!;
            sellerRequest.RequestType = _sellerRequestTypeService.GetById(sellerRequest.RequestTypeId)!;
            if (_sellerRequestTypeService.IsAddType(sellerRequest))
            {
                T? entity = JsonSerializer.Deserialize<T>(sellerRequest.Content);
                addMethod.Invoke(entity);
            }
            else if (_sellerRequestTypeService.IsUpdateType(sellerRequest))
            {
                T? entity = JsonSerializer.Deserialize<T>(sellerRequest.Content);
                updateMethod.Invoke(entity);
            }
            _sellerRequestRepository.Update(sellerRequest);
        }

        public void RejectRequest(long requestId)
        {
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
