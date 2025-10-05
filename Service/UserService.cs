
using Repository;
using Repository.Repository;

namespace Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository = new();

        public User? Login(string email, string password)
        {
            var users = _userRepository.GetByCondition(u => u.Email == email && u.Password == password);
            return users.FirstOrDefault();
        }
    }
}
