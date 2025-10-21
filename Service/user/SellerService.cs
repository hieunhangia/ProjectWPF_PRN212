using Repository.Models.user;
using Repository.Repository.user;
using System.Linq.Expressions;
using System.Net.Mail;

namespace Service.user
{
    public class SellerService(SellerRepository sellerRepository,
        AddressService addressService)
    {
        private readonly SellerRepository _sellerRepository = sellerRepository;
        private readonly AddressService _addressService = addressService;

        public List<Seller> GetAllSellers() => _sellerRepository.GetAll();

        public Seller? GetSellerById(long id) => _sellerRepository.GetById(id);

        public List<Seller> GetSellersByCondition(Expression<Func<Seller, bool>> condition) => _sellerRepository.GetByCondition(condition);

        public void AddNewSeller(string email,
            string password,
            string fullName,
            DateOnly? birthDate,
            string identify,
            string provinceCity,
            string communneWard,
            string specificAddress,
            bool isActive)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName) ||
                birthDate == null ||
                string.IsNullOrWhiteSpace(identify) ||
                provinceCity == "default" ||
                communneWard == "default" ||
                string.IsNullOrWhiteSpace(specificAddress))
            {
                throw new Exception("Có một hoặc nhiều thông tin chưa được nhập.");
            }
            if (!IsValidEmail(email))
            {
                throw new Exception("Email không hợp lệ.");
            }

            if (!IsValidPassword(password))
            {
                throw new Exception("Mật khẩu phải trong khoảng 6-50 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.");
            }

            if (birthDate > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Ngày sinh không hợp lệ.");
            }

            if (!IsValidID(identify))
            {
                throw new Exception("CMND/CCCD phải là 9 hoặc 12 số.");
            }

            if (IsEmailExists(email))
            {
                throw new Exception("Email đã tồn tại.");
            }

            if (IsIdentifyExists(identify))
            {
                throw new Exception("CMND/CCCD đã tồn tại.");
            }

            Seller seller = new()
            {
                Email = email,
                Password = password,
                IsActive = isActive,
                FullName = fullName,
                BirthDate = birthDate.Value,
                Cid = identify,
                SpecificAddress = specificAddress,
                CommuneWardCode = communneWard
            };
            _sellerRepository.Add(seller);
        }

        public void UpdateExistingSeller(Seller sellerToUpdate,
            string? password,
            string fullName,
            DateOnly? birthDate,
            string identify,
            string provinceCity,
            string communneWard,
            string specificAddress,
            bool isActive)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName) ||
                birthDate == null ||
                string.IsNullOrWhiteSpace(identify) ||
                provinceCity == "default" ||
                communneWard == "default" ||
                string.IsNullOrWhiteSpace(specificAddress))
            {
                throw new Exception("Có một hoặc nhiều thông tin chưa được nhập.");
            }

            if (!IsValidPassword(password))
            {
                throw new Exception("Mật khẩu phải trong khoảng 6-50 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.");
            }

            if (birthDate > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Ngày sinh không hợp lệ.");
            }

            if (!IsValidID(identify))
            {
                throw new Exception("CMND/CCCD phải là 9 hoặc 12 số.");
            }

            if (IsIdentifyExists(identify, sellerToUpdate.Id))
            {
                throw new Exception("CMND/CCCD đã tồn tại.");
            }

            sellerToUpdate.Password = password;
            sellerToUpdate.FullName = fullName;
            sellerToUpdate.BirthDate = birthDate.Value;
            sellerToUpdate.Cid = identify;
            sellerToUpdate.SpecificAddress = specificAddress;
            sellerToUpdate.CommuneWard = _addressService.GetCommuneWardByCode(communneWard);
            sellerToUpdate.IsActive = isActive;
            _sellerRepository.Update(sellerToUpdate);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPassword(string password) =>
                password.Length >= 6
                && password.Length <= 50
                && password.Any(char.IsLower)
                && password.Any(char.IsUpper)
                && password.Any(char.IsDigit)
                && password.Any(ch => !char.IsLetterOrDigit(ch));

        private static bool IsValidID(string identify) => (identify.Length == 9 || identify.Length == 12) && identify.All(char.IsDigit);

        private bool IsEmailExists(string email) => _sellerRepository.GetByCondition(s => s.Email == email).FirstOrDefault() != null;

        private bool IsIdentifyExists(string identify) => _sellerRepository.GetByCondition(s => s.Cid == identify).FirstOrDefault() != null;
        private bool IsIdentifyExists(string identify, long excludeSellerId)
        {
            return _sellerRepository.GetByCondition(s => s.Cid == identify && s.Id != excludeSellerId).FirstOrDefault() != null;
        }

        public void AddSeller(Seller seller) => _sellerRepository.Add(seller);

        public void UpdateSeller(Seller seller) => _sellerRepository.Update(seller);

        public void DeleteSeller(long sellerId) => _sellerRepository.Delete(sellerId);
    }
}
