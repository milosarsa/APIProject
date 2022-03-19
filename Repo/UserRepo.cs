using Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class UserRepo : IUserRepo
    {
        private List<UserDetails> _users = new List<UserDetails>
        {
            new UserDetails {Id = Guid.NewGuid(), Name = "Admin", LastName = "Test", Username = "admin", Password = "admin", UserRole = UserRoles.Admin},
            new UserDetails {Id = Guid.NewGuid(), Name = "Manager", LastName = "Test", Username = "manager", Password = "manager", UserRole = UserRoles.Manager},
            new UserDetails {Id = Guid.NewGuid(), Name = "User", LastName = "Test", Username = "test", Password = "test", UserRole = UserRoles.User},
        };

        public bool IsValidUser(UserCreds? userCreds, out UserAuth userAccount)
        {
            UserDetails user = _users.SingleOrDefault(x =>
            {
                return x.Username == userCreds.Username&& x.Password == userCreds.Password;
            }) ?? new UserDetails();


            if (user == null)
            {
                userAccount = new UserAuth();
                return false;
            }

            //We are passing user auth details without a token as it will be generated later
            userAccount = new UserAuth(user.Id,user.Username, user.UserRole.GetName());
            return true;
        }
    }
}
