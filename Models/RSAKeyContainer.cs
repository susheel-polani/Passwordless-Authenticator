using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Models
{
    internal class RSAKeyContainer
    {
        // Put this file in Utils
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
    }
}
