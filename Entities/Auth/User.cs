using Entities.Enums;

namespace Entities.Auth
{

    public class UserDetails
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
    }
    public class User : UserDetails
    {
        public Guid Id { get; set; }
        public string? Password { get; set; }
        public UserRoles UserRole { get; set; }
    }

    //Using records for user auth details to avoid copying data but only use a reference to the value
    //Switched from record to record class 
    public record class UserAuth
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
        public string? Token { get; set; }

        public UserAuth()
        {

        }

        public UserAuth(Guid Id, string? Username, string? UserRole)
        {
            this.Id = Id;
            this.UserName = Username;
            this.UserRole = UserRole;
        }

        public UserAuth(Guid Id, string? Username, string? UserRole, string? Token)
        {
            this.Id = Id;
            this.UserName = Username;
            this.UserRole = UserRole;
            this.Token = Token;
        }

        public static UserAuth Empty
        {
            get
            {
                Guid guid = Guid.NewGuid();
                string username = String.Empty;
                string userrole = String.Empty;
                string token = String.Empty;
                return new UserAuth(guid, username, userrole, token);
            }
        }
    }

    public record UserCreds(string? Username, string? Password);


    


}
