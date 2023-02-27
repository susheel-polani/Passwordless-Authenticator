using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Utils.Attributes;
using Passwordless_Authenticator.Constants.HTTPServer;
using Passwordless_Authenticator.Models;
using Passwordless_Authenticator.Utils;

namespace Passwordless_Authenticator.Services.WebInterfaceApis
{
    [Route("/crypto")]
    internal class CryptoApis
    {
      
        [GetEndpoint("/encrypt-message")]
        public void getEncryptedMessageHandler(HttpListenerContext context) {
            Debug.WriteLine("GET /encrypt-message");
            
            HttpListenerRequest request = context.Request;

            Debug.WriteLine(WebInterfaceServerUtils.getPayload(request));



            var response = context.Response;

            var responsePayload = new ResponsePayload();
            responsePayload.isSuccess = true;
            responsePayload.message = "Test";
            responsePayload.payload = new JObject();
            responsePayload.payload.Add("Hello", "World");

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_DESC_OK;

            WebInterfaceServerUtils.sendResponse(response, responsePayload);


        }
    }
}
