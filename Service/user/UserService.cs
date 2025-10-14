using Repository.Models.user;
using Repository.Repository.user;

namespace Service.user
{
    public class UserService(UserRepository userRepository)
    {
        private readonly UserRepository _userRepository = userRepository;

        public User Login(string email, string password)
        {
            var user = _userRepository.GetByCondition(u => u.Email == email && u.Password == password).FirstOrDefault();

            if (user != null)
            {
                if (user.IsActive)
                {
                    return user;
                }
                else
                {
                    throw new Exception("Tài khoản của bạn đã bị vô hiệu hóa.");
                }
            }
            else
            {
                throw new Exception("Email hoặc mật khẩu không đúng");
            }
        }
    }
}
