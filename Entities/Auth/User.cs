using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;

namespace Entities.Auth
{

    //Using records for user auth details to avoid copying data but only use a reference to the value
    public record UserAuth(Guid Id, string Username, string UserRole, string Token);
    public record UserCreds(string Username, string Password);

    public class UserDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRoles UserRole { get; set; }
    }
    
}
