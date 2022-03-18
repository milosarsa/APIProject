using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Auth;

namespace Interfaces.Service
{
    public interface IUserService
    {
        public bool Authenticate(UserCreds userCreds, out UserAuth userAuth);
    }
}
