using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace APIProject
{
    public class MinAccessLevelRequirement : IAuthorizationRequirement
    {
        public MinAccessLevelRequirement(int accessLevel) =>
            AccessLevel = accessLevel;

        int AccessLevel { get; }
    }
}
