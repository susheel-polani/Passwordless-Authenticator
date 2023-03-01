using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Constants.HTTPServer;
using Passwordless_Authenticator.Dao_controllers;
using Passwordless_Authenticator.Models;
using Passwordless_Authenticator.Services.Auth;
using Passwordless_Authenticator.Services.Keys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Services.SignUp
{
    internal class ClientAuthServices
    {
        public static async Task<ResponsePayload> SignUp(string domainName, string userName, HttpListenerContext context)
        {
            string containerName = domainName + userName;
            Debug.WriteLine("Container Name: " + containerName);

            WindowsAuthData result = await AppAuthenticationService.authenticate();
            var responsePayload = new ResponsePayload();
            var responseContext = context.Response;
            if (result.flag)
            {
                if(!UserAuthDataController.insertUserData(domainName, userName, containerName))
                {
                    responsePayload.isSuccess = false;
                    responsePayload.message = WebInterfaceServerConstants.USER_ALREADY_EXISTS;
                    responseContext.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseContext.StatusDescription = WebInterfaceServerConstants.ERR_HTTP_INTERNAL_ERROR;
                }
                else
                {
                    JObject response = RSAKeyServices.GenerateKeyInContainer(containerName);
                    responsePayload.isSuccess = true;
                    responsePayload.message = WebInterfaceServerConstants.KEY_GEN_SUCCESS;
                    responsePayload.payload = response;

                    responseContext.StatusCode = (int)HttpStatusCode.OK;
                    responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_DESC_OK;
                }
            }
            else
            {
                responsePayload.isSuccess = false;
                responsePayload.message = WebInterfaceServerConstants.WINDOWS_AUTH_FAIL;
                responseContext.StatusCode = (int)HttpStatusCode.Forbidden;
                responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_UNAUTHORIZED;
            }
            return responsePayload;
        }

        public static async Task<ResponsePayload> fetchUsernames(string domainName, HttpListenerContext context)
        {
            List<JObject> rows = UserAuthDataController.getUsernames(domainName);
            JArray usernameList = new JArray();
            for(int i=0;i < rows.Count;i++)
            {
                usernameList.Add(rows[i].GetValue("username").ToString());
            }
            JObject result = new JObject(new JProperty("usernames", usernameList));
            Debug.WriteLine(JsonConvert.SerializeObject(result));

            var responsePayload = new ResponsePayload();
            var responseContext = context.Response;
            responsePayload.isSuccess = true;
            responsePayload.message = WebInterfaceServerConstants.USERNAME_FETCH_SUCCESS;
            responsePayload.payload = result;
            responseContext.StatusCode = (int)HttpStatusCode.OK;
            responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_DESC_OK;

            return responsePayload;
        }
    }
}
