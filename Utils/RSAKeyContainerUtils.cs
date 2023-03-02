using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Utils
{
    internal class RSAKeyContainerUtils
    {
        public static RSA fetchContainer(string containerName)
        {
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };
            var rsa = new RSACryptoServiceProvider(parameters);

            return rsa;
        }
        public static RSA deleteContainer(string containerName)
        {
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };
            var rsa = new RSACryptoServiceProvider(parameters)
            {
                PersistKeyInCsp = false
            };
            return rsa;
        }
        public static bool doesKeyExist(string containerName)
        {
            var parameters = new CspParameters
            {
                Flags = CspProviderFlags.UseExistingKey,
                KeyContainerName = containerName
            };
            try
            {
                var rsa = new RSACryptoServiceProvider(parameters);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
