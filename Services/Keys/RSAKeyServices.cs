using Newtonsoft.Json;
using Passwordless_Authenticator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Services.Keys
{
    internal class RSAKeyServices
    {
        public class JSONKey
        {
            public string modulus { get; set; }
            public string exponent { get; set; }
            public string p { get; set; }
            public string q { get; set; }
            public string dp { get; set; }
            public string dq { get; set; }
            public string inverseq { get; set; }
            public string d { get; set; }
        }
        public static string getPubKeyParameters(RSA rsa)
        {
            RSAParameters pubkeyParameters = rsa.ExportParameters(false);

            JSONKey publicKey = new JSONKey()
            {
                modulus = (pubkeyParameters.Modulus != null ? Convert.ToBase64String(pubkeyParameters.Modulus) : null),
                exponent = (pubkeyParameters.Exponent != null ? Convert.ToBase64String(pubkeyParameters.Exponent) : null)
            };

            string stringjson = JsonConvert.SerializeObject(publicKey);

            return stringjson;
        }

        public static string getPriKeyParameters(RSA rsa)
        {
            RSAParameters prikeyParameters = rsa.ExportParameters(true);

            JSONKey privateKey = new JSONKey()
            {
                modulus = (prikeyParameters.Modulus != null ? Convert.ToBase64String(prikeyParameters.Modulus) : null),
                exponent = (prikeyParameters.Exponent != null ? Convert.ToBase64String(prikeyParameters.Exponent) : null),
                p = prikeyParameters.P != null ? Convert.ToBase64String(prikeyParameters.P) : null,
                q = prikeyParameters.Q != null ? Convert.ToBase64String(prikeyParameters.Q) : null,
                dp = prikeyParameters.DP != null ? Convert.ToBase64String(prikeyParameters.DP) : null,
                dq = prikeyParameters.DQ != null ? Convert.ToBase64String(prikeyParameters.DQ) : null,
                inverseq = prikeyParameters.InverseQ != null ? Convert.ToBase64String(prikeyParameters.InverseQ) : null,
                d = prikeyParameters.D != null ? Convert.ToBase64String(prikeyParameters.D) : null

            };

            string stringjson = JsonConvert.SerializeObject(privateKey);

            return stringjson;
        }
        public static string GenerateKeyInContainer(string containerName)
        {
            // fetchContainer function will create a container if it does not exist or will fetch the existing container.
            var rsa = RSAKeyContainer.fetchContainer(containerName);
            return getPubKeyParameters(rsa);
        }

        public static string GetPrivateKeyFromContainer(string containerName)
        {
            var rsa = RSAKeyContainer.fetchContainer(containerName);
            return getPriKeyParameters(rsa);
        }
        public static void DeleteKeyFromContainer(string containerName)
        {
            var rsa = RSAKeyContainer.deleteContainer(containerName);
            rsa.Clear();
        }

        public static void exportKey()
        {
            DeleteKeyFromContainer("test_container");
            DeleteKeyFromContainer("target_container");

            string pubkey = GenerateKeyInContainer("test_container");
            Debug.WriteLine("Public key:" + pubkey);
            string prikey = GetPrivateKeyFromContainer("test_container");
            Debug.WriteLine("Private key:" + prikey);

            var rsa = RSAKeyContainer.fetchContainer("test_container");
            string exportkey = rsa.ToXmlString(true);
            Debug.WriteLine("Exported key:" + exportkey);
            var rsa2 = RSAKeyContainer.fetchContainer("target_container");
            rsa2.FromXmlString(exportkey);

            string pubkey2 = GenerateKeyInContainer("target_container");
            Debug.WriteLine("Public key2:" + pubkey2);
            string prikey2 = GetPrivateKeyFromContainer("target_container");
            Debug.WriteLine("Private key2:" + prikey2);

        }
    }
}
