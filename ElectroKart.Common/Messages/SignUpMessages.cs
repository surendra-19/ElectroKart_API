using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.Messages
{
    public class SignUpMessages
    {
        public const string SignUpSuccess = "User registered successfully.";
        public const string EmailAlreadyRegistered = "A user with this email already exists.";
        public const string ServerError = "An error occurred while processing your request. Please try again later.";
        public const string PhoneAlreadyExists = "A user with this Phone number already exists.";
    }
}
