using Entities.Auth;

namespace Interfaces.Repo
{
    public interface IUserRepo
    {
        public bool IsValidUser(UserCreds userCreds, out UserAuth userAccount);
    }
}
