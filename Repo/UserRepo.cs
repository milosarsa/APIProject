using Entities.Auth;

namespace Repo
{
    public class UserRepo : IUserRepo
    {
        private List<User> _users = new List<User>
        {
            new User {Id = Guid.NewGuid(), Name = "Admin", LastName = "Test", Username = "admin", Password = "admin", UserRole = UserRoles.Admin},
            new User {Id = Guid.NewGuid(), Name = "Manager", LastName = "Test", Username = "manager", Password = "manager", UserRole = UserRoles.Manager},
            new User {Id = Guid.NewGuid(), Name = "User", LastName = "Test", Username = "test", Password = "test", UserRole = UserRoles.User},
        };

        public bool IsValidUser(UserCreds? userCreds, out UserAuth userAccount)
        {
            //Returns true if list contains the user
            //
            User? user = _users.SingleOrDefault(x =>
            {
                return x.Username == userCreds.Username && x.Password == userCreds.Password;
            });


            if (user == null)
            {
                userAccount = UserAuth.Empty;
                return false;
            }

            //We are passing user auth details without a token as it will be generated later
            userAccount = new UserAuth(user.Id, user.Username, user.UserRole.GetName());
            return true;
        }

        public UserDetails? Read(string username)
        {
            if(username == null)
                return null;

            UserDetails userDetails = _users.SingleOrDefault(x =>
            {
                return x.Username == username;
            }) as UserDetails;

            return userDetails;
        }
    }
}
