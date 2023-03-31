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

            WindowsAuthData result = await AppAuthenticationService.authenticateUser("User authentication needed before Signing up to " + domainName + " via the " + userName + " username");
            var responsePayload = new ResponsePayload();
            var responseContext = context.Response;
            if (result.flag)
            {
                if (!UserAuthDataController.insertUserData(domainName, userName, containerName))
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
                responsePayload.message = result.message;
                responseContext.StatusCode = (int)HttpStatusCode.Forbidden;
                responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_UNAUTHORIZED;
            }
            return responsePayload;
        }

        public static async Task<ResponsePayload> fetchUsernames(string domainName, HttpListenerContext context)
        {
            List<JObject> rows = UserAuthDataController.getUsernames(domainName);
            JArray usernameList = new JArray();
            for (int i = 0; i < rows.Count; i++)
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

        public static async Task<ResponsePayload> encryptServerMessage(string domainName, string userName, string serverMessage, HttpListenerContext context)
        {
            string containerName = domainName + userName;
            WindowsAuthData authenticateResult = await AppAuthenticationService.authenticateUser("User authentication needed for authentication to " + domainName + " via the " + userName + " username");
            var responsePayload = new ResponsePayload();
            var responseContext = context.Response;
            if (authenticateResult.flag)
            {
                string cipherText = RSAKeyServices.AsymmEncrypt(containerName, serverMessage);
                JObject result = new JObject();
                result.Add("cipherText", cipherText);

                responsePayload.isSuccess = true;
                responsePayload.message = WebInterfaceServerConstants.SEVER_MSG_ENCRYPT_SUCCESS;
                responsePayload.payload = result;
                responseContext.StatusCode = (int)HttpStatusCode.OK;
                responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_DESC_OK;
            }
            else
            {
                responsePayload.isSuccess = false;
                responsePayload.message = authenticateResult.message;
                responseContext.StatusCode = (int)HttpStatusCode.Forbidden;
                responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_UNAUTHORIZED;
            }
            return responsePayload;
        }

        public static async Task<ResponsePayload> getPublicKey(string domainName, string userName, HttpListenerContext context)
        {
            string containerName = domainName + userName;
            JObject result = RSAKeyServices.GetPublicKeyFromContainer(containerName);
            var responsePayload = new ResponsePayload();
            var responseContext = context.Response;
            if (result!=null)
            {
                responsePayload.isSuccess = true;
                responsePayload.message = WebInterfaceServerConstants.PUB_KEY_FETCH_SUCCESS;
                responsePayload.payload = result;
                responseContext.StatusCode = (int)HttpStatusCode.OK;
                responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_DESC_OK;
            }
            else
            {
                responsePayload.isSuccess = false;
                responsePayload.message = WebInterfaceServerConstants.PUB_KEY_DOES_NOT_EXIST;
                responseContext.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseContext.StatusDescription = WebInterfaceServerConstants.ERR_HTTP_INTERNAL_ERROR;
            }
            return responsePayload;
        }

        public static async Task<ResponsePayload> deleteUser(string domainName, string userName, HttpListenerContext context)
        {
            string containerName = domainName + userName;
            Debug.WriteLine("Container Name: " + containerName);

            WindowsAuthData result = await AppAuthenticationService.authenticateUser("User authentication needed before deleting the " + userName + " account from  " + domainName);
            var responsePayload = new ResponsePayload();
            var responseContext = context.Response;
            if (result.flag)
            {
                if (!UserAuthDataController.deleteUsername(containerName))
                {
                    responsePayload.isSuccess = false;
                    responsePayload.message = WebInterfaceServerConstants.USER_DOES_NOT_EXISTS;
                    responseContext.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseContext.StatusDescription = WebInterfaceServerConstants.ERR_HTTP_INTERNAL_ERROR;
                }
                else
                {
                    RSAKeyServices.DeleteKeyFromContainer(containerName);
                    responsePayload.isSuccess = true;
                    responsePayload.message = WebInterfaceServerConstants.USERNAME_DELETE_SUCCESS;

                    responseContext.StatusCode = (int)HttpStatusCode.OK;
                    responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_DESC_OK;
                }
            }
            else
            {
                responsePayload.isSuccess = false;
                responsePayload.message = result.message;
                responseContext.StatusCode = (int)HttpStatusCode.Forbidden;
                responseContext.StatusDescription = WebInterfaceServerConstants.HTTP_RESP_UNAUTHORIZED;
            }
            return responsePayload;
        }
    }
}
