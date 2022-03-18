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
            new UserDetails {Id = Guid.NewGuid(), Name = "Milos", LastName = "Arsenijevic", Username = "admin", Password = "admin", UserRole = UserRoles.Admin},
            new UserDetails {Id = Guid.NewGuid(), Name = "Milos", LastName = "Arsenijevic", Username = "test", Password = "test", UserRole = UserRoles.User},
        };

        public bool IsValidUser(UserCreds userCreds, out UserAuth userAccount)
        {
            UserDetails user = _users.SingleOrDefault(x => x.Username == userCreds.Username && x.Password == userCreds.Password);
            if (user == null)
            {
                userAccount = new UserAuth(Guid.Empty, "", Enum.GetName<UserRoles>(UserRoles.Unauthorized), "");
                return false;
            }

            userAccount = new UserAuth(user.Id,user.Username, Enum.GetName<UserRoles>(user.UserRole), "");
            return true;
        }
    }
}
