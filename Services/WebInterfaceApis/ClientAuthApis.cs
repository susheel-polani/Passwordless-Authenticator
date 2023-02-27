using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Constants.HTTPServer;
using Passwordless_Authenticator.Models;
using Passwordless_Authenticator.Services.SignUp;
using Passwordless_Authenticator.Utils;
using Passwordless_Authenticator.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Services.WebInterfaceApis
{
    [Route("/client-auth")]
    internal class ClientAuthApis
    {
        [PostEndpoint("/signup")]
        public async void  signUp(HttpListenerContext context)
        {
            Debug.WriteLine("POST /client-auth/signup");

            HttpListenerRequest request = context.Request;

            var requestParams = WebInterfaceServerUtils.getPayload(request);

            Debug.WriteLine("Domain: " + requestParams.GetValue("domain"));
            Debug.WriteLine("Username: " + requestParams.GetValue("username"));

            string containerName = requestParams.GetValue("domain").ToString() + requestParams.GetValue("username").ToString();
            Debug.WriteLine("Container Name: " + containerName);

            var responsePayload = new ResponsePayload();
            responsePayload = await ClientAuthServices.SignUp(containerName);
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_DESC_OK;

            WebInterfaceServerUtils.sendResponse(response, responsePayload);
        }
    }
}
