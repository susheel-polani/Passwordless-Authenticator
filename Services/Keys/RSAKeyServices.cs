using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Models;
using Passwordless_Authenticator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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
        public static JObject getPubKeyParameters(RSA rsa)
        {
            RSAParameters pubkeyParameters = rsa.ExportParameters(false);

            JObject publicKey = new JObject();
            publicKey.Add("modulus", pubkeyParameters.Modulus != null ? Convert.ToBase64String(pubkeyParameters.Modulus) : null);
            publicKey.Add("exponent", pubkeyParameters.Exponent != null ? Convert.ToBase64String(pubkeyParameters.Exponent) : null);

            return publicKey;
        }

        public static JObject getPriKeyParameters(RSA rsa)
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

            return JObject.FromObject(privateKey);
        }
        public static JObject GenerateKeyInContainer(string containerName)
        {
            // fetchContainer function will create a container if it does not exist or will fetch the existing container.
            var rsa = RSAKeyContainerUtils.fetchContainer(containerName);
            return getPubKeyParameters(rsa);
        }

        public static JObject GetPublicKeyFromContainer(string containerName)
        {
            if(RSAKeyContainerUtils.doesKeyExist(containerName))
            {
                var rsa = RSAKeyContainerUtils.fetchContainer(containerName);
                return getPubKeyParameters(rsa);
            }
            return null;
        }

        public static JObject GetPrivateKeyFromContainer(string containerName)
        {
            var rsa = RSAKeyContainerUtils.fetchContainer(containerName);
            return getPriKeyParameters(rsa);
        }
        public static void DeleteKeyFromContainer(string containerName)
        {
            var rsa = RSAKeyContainerUtils.deleteContainer(containerName);
            rsa.Clear();
        }
        public static string AsymmEncrypt(string containerName, string plaintext)
        {
            var rsa = RSAKeyContainerUtils.fetchContainer(containerName);

            byte[] encryptedAsBytes = rsa.SignData(Encoding.UTF8.GetBytes(plaintext), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1); //rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.OaepSHA1);
            string encryptedAsBase64 = Convert.ToBase64String(encryptedAsBytes);
            return encryptedAsBase64;
        }
    }
}
