using Repository.Models.user;
using Repository.Repository.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.user
{
    public class UserTokenService(UserTokenRepository userTokenRepository)
    {

        private const string _filePath = "user_token.dat";

        [SupportedOSPlatform("windows")]
        public void SaveUserToken(long userId, DateTime expiration)
        {
            var token = Guid.NewGuid().ToString();
            var userToken = userTokenRepository.GetById(userId);
            if (userToken != null)
            {
                userToken.Token = token;
                userToken.Expiration = expiration;
                userTokenRepository.Update(userToken);
            }
            else
            {
                userToken = new UserToken
                {
                    UserId = userId,
                    Token = token,
                    Expiration = expiration
                };
                userTokenRepository.Add(userToken);
            }
            File.WriteAllBytes(_filePath, ProtectedData.Protect(JsonSerializer.SerializeToUtf8Bytes(userToken), null, DataProtectionScope.CurrentUser));
        }

        public void DeleteUserToken(long userId)
        {
            userTokenRepository.Delete(userId);
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        [SupportedOSPlatform("windows")]
        public User? GetUserIfValidTokenExist()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    var fileUserToken = JsonSerializer.Deserialize<UserToken>(ProtectedData.Unprotect(File.ReadAllBytes(_filePath), null, DataProtectionScope.CurrentUser))!;
                    return userTokenRepository
                        .GetByCondition(ut => ut.UserId == fileUserToken.UserId && ut.Token == fileUserToken.Token && ut.Expiration > DateTime.Now)
                        .FirstOrDefault()?.User;
                }
                catch
                {
                }
            }
            return null;
        }
    }
}
