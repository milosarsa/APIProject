using Entities.Auth;

namespace Interfaces.Service
{
    public interface IUserService
    {
        public bool Authenticate(UserCreds userCreds, out UserAuth userAuth);
        public UserDetails GetUser(string username);
    }
}
