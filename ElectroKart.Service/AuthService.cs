using ElectroKart.Common.Data;
using ElectroKart.Common.DTOS;
using ElectroKart.Common.Models;
using ElectroKart.DataAccess;

namespace ElectroKart.Service
{
    public class AuthService
    {
        private readonly Context _dbcontext;
        private readonly AuthDataAccess _authDataAccess;
        public AuthService(Context context,AuthDataAccess authDataAccess)
        {
            _dbcontext = context;
            _authDataAccess = authDataAccess;
        }
        public async Task<LoginResult> LoginUser(LoginDTO logindto)
        {
            return await _authDataAccess.LoginUser(logindto);
        }
    }
}
