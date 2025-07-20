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
        public async Task<bool> IsEmailRegistered(string email)
        {
            return await _authDataAccess.IsEmailRegistered(email);
        }
        public async Task<bool> IsPhoneRegistered(string phone)
        {
            return await _authDataAccess.IsPhoneRegistered(phone);
        }
        public async Task<int> RegisterUser(SignUpDTO signUpDTO)
        {
            return await _authDataAccess.RegisterUser(signUpDTO);
        } 

    }
}
