﻿using ElectroKart.Common.Data;
using ElectroKart.Common.DTOS;
using ElectroKart.Common.Models;
using ElectroKart.DataAccess;

namespace ElectroKart.Service
{
    public class AuthorizationService
    {
        private readonly Context _dbcontext;
        private readonly AuthorizationDataAccess _authDataAccess;
        public AuthorizationService(Context context,AuthorizationDataAccess authDataAccess)
        {
            _dbcontext = context;
            _authDataAccess = authDataAccess;
        }
        public async Task<LoginResult> LoginUser(LoginDTO logindto)
        {
            return await _authDataAccess.LoginUser(logindto);
        }
        public async Task<bool> IsEmailRegistered(string email,int? Cust_Id)
        {
            return await _authDataAccess.IsEmailRegistered(email, Cust_Id);
        }
        public async Task<bool> IsPhoneRegistered(string phone,int? Cust_Id)
        {
            return await _authDataAccess.IsPhoneRegistered(phone, Cust_Id);
        }
        public async Task<int> RegisterUser(SignUpDTO signUpDTO)
        {
            return await _authDataAccess.RegisterUser(signUpDTO);
        } 

    }
}
