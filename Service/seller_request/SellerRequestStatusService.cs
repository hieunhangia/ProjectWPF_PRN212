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
    public class SellerRequestStatusService(SellerRequestStatusRepository sellerRequestStatusRepository)
    {
        private readonly SellerRequestStatusRepository _sellerRequestStatusRepository = sellerRequestStatusRepository;

        public SellerRequestStatus? GetPendingStatus()
        {
            return _sellerRequestStatusRepository.GetByCondition(s => s.Name == "Đang Chờ Duyệt").FirstOrDefault();
        }

        public SellerRequestStatus? GetApprovedStatus()
        {
            return _sellerRequestStatusRepository.GetByCondition(s => s.Name == "Đã Duyệt").FirstOrDefault();
        }

        public SellerRequestStatus? GetRejectedStatus()
        {
            return _sellerRequestStatusRepository.GetByCondition(s => s.Name == "Đã Từ Chối").FirstOrDefault();
        }

        public bool IsPendingStatus(SellerRequest sellerRequest)
        {
            return sellerRequest.Status.Name == "Đang Chờ Duyệt";
        }
    }
}
