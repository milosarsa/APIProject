using Entities.Enums;

namespace Entities.Auth
{

    //Using records for user auth details to avoid copying data but only use a reference to the value
    //Switched from record to record class 
    public record class UserAuth
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
        public string? Token { get; set; }

        public UserAuth(){}

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
    }

    public record UserCreds(string? Username, string? Password);

    public class UserDetails
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public UserRoles UserRole { get; set; }
    }

}
