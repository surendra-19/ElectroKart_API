using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.Messages
{
    public class LoginMessages
    {
        public const string LoginSuccess = "Login successful.";
        public const string IncorrectPassword = "The password you entered is incorrect.";
        public const string UserNotFound = "No user found with the provided credentials.";
        public const string ServerError = "An error occurred while processing your request.";
    }
}

