using Microsoft.UI.Xaml.Controls;
using Passwordless_Authenticator.Services.Crypto;
using Passwordless_Authenticator.Services.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Services.Auth
{
    internal class PasswordService
    {
        public static bool checkPassword(string inputPassword)
        {

            string passHash = PasswordDB.GetPassword();

            string inputPassHash = CryptoUtils.hashData(inputPassword);

            if (passHash == inputPassHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkKey(string inputKey)
        {

            string keyHash = PasswordDB.GetKey();

            string inputKeyHash = CryptoUtils.hashData(inputKey);

            if (keyHash == inputKeyHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
