using Microsoft.Extensions.DependencyInjection;
using Repository.Models.user;
using Service.user;

namespace Controller
{
    public class UserController(UserService userService)
    {
        private readonly UserService _userService = userService;

        public User Login(string email, string password)
        {
            var user = _userService.Login(email,password);

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
                throw new Exception("Tên tài khoản hoặc mật khẩu không đúng");
            }
        }
    }
}
