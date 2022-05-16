using Entities.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class UserService : IUserService
    {
        private IUserRepo userRepo;
        private ILogger<UserService> logger;
        private readonly string encodingString;
        public UserService(IUserRepo _userRepo, ILogger<UserService> _logger, IConfiguration _configuration)
        {
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
            userRepo = _userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            encodingString = _configuration["EncodingString"];
        }

        public UserDetails GetUser(string username)
        {
            UserDetails userDetails = userRepo.Read(username);

            return userDetails;

        }


        private string GenerateToken(UserAuth userAuth)
        {


            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userAuth.Id.ToString()),
                new Claim(ClaimTypes.Name, userAuth.UserName),
                new Claim(ClaimTypes.Role, userAuth.UserRole),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString())
            };

            var header = new JwtHeader(
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(encodingString)),
                        SecurityAlgorithms.HmacSha256));

            var payload = new JwtPayload(claim);
            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool Authenticate(UserCreds _userCreds, out UserAuth _userAuth)
        {
            if (userRepo.IsValidUser(_userCreds, out _userAuth))
                if(_userAuth != UserAuth.Empty)
                {
                    string token = GenerateToken(_userAuth);
                    _userAuth = _userAuth with { Token = token };
                    return true;
                }
            return false;
            //new UserAuth(_userAuth.Id, _userAuth.Username, _userAuth.UserRole, token);

        }
    }
}
