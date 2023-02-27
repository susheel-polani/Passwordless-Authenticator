using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Models;
using Passwordless_Authenticator.Services.Auth;
using Passwordless_Authenticator.Services.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Services.SignUp
{
    internal class ClientAuthServices
    {
        public static async Task<ResponsePayload> SignUp(string containerName)
        {
            WindowsAuthData result = await AppAuthenticationService.authenticate();
            var responsePayload = new ResponsePayload();
            if(result.flag)
            {
                JObject response = RSAKeyServices.GenerateKeyInContainer(containerName);
                responsePayload.isSuccess = true;
                responsePayload.message = "Key Pair Successfully generated!";
                responsePayload.payload = response;
            }
            else
            {
                responsePayload.isSuccess = false;
                responsePayload.message = "Windows Authentication Failed!";
            }
            return responsePayload;
        }
    }
}
