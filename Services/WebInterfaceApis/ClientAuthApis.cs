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
        public async void signUp(HttpListenerContext context)
        {
            Debug.WriteLine("POST /client-auth/signup");

            HttpListenerRequest request = context.Request;
            var requestParams = WebInterfaceServerUtils.getPayload(request);

            string domainName = requestParams.GetValue("domain").ToString();
            string userName = requestParams.GetValue("username").ToString();
            Debug.WriteLine("Domain: " + domainName);
            Debug.WriteLine("Username: " + userName);

            var responsePayload = new ResponsePayload();
            responsePayload = await ClientAuthServices.SignUp(domainName, userName, context);
            var response = context.Response;

            WebInterfaceServerUtils.sendResponse(response, responsePayload);
        }

        [GetEndpoint("/existing-usernames")]
        public async void existingUsernames(HttpListenerContext context)
        {
            Debug.WriteLine("GET /client-auth/existing-usernames");

            HttpListenerRequest request = context.Request;
            var requestParams = WebInterfaceServerUtils.getPayload(request);

            string domainName = requestParams.GetValue("domain").ToString();
            Debug.WriteLine("Domain: " + domainName);
            var responsePayload = new ResponsePayload();
            responsePayload = await ClientAuthServices.fetchUsernames(domainName, context);
            var response = context.Response;

            WebInterfaceServerUtils.sendResponse(response, responsePayload);
        }

        [PostEndpoint("/encrypt-message")]
        public async void encryptMessage(HttpListenerContext context)
        {
            Debug.WriteLine("POST /client-auth/encrypt-message");

            HttpListenerRequest request = context.Request;
            var requestParams = WebInterfaceServerUtils.getPayload(request);

            string domainName = requestParams.GetValue("domain").ToString();
            string userName = requestParams.GetValue("username").ToString();
            string serverMessage = requestParams.GetValue("serverMessage").ToString();
            Debug.WriteLine("Domain: " + domainName);
            Debug.WriteLine("Username: " + userName);
            Debug.WriteLine("Server Message: " + serverMessage);

            var responsePayload = new ResponsePayload();
            responsePayload = await ClientAuthServices.encryptServerMessage(domainName, userName, serverMessage, context);
            var response = context.Response;

            WebInterfaceServerUtils.sendResponse(response, responsePayload);
        }

        [PostEndpoint("/get-key")]
        public async void getKey(HttpListenerContext context)
        {
            Debug.WriteLine("POST /client-auth/get-key");

            HttpListenerRequest request = context.Request;
            var requestParams = WebInterfaceServerUtils.getPayload(request);

            string domainName = requestParams.GetValue("domain").ToString();
            string userName = requestParams.GetValue("username").ToString();
            Debug.WriteLine("Domain: " + domainName);
            Debug.WriteLine("Username: " + userName);

            var responsePayload = new ResponsePayload();
            responsePayload = await ClientAuthServices.getPublicKey(domainName, userName, context);
            var response = context.Response;

            WebInterfaceServerUtils.sendResponse(response, responsePayload);
        }

        [PostEndpoint("/delete-user")]
        public async void deleteUser(HttpListenerContext context)
        {
            Debug.WriteLine("POST /client-auth/delete-user");

            HttpListenerRequest request = context.Request;
            var requestParams = WebInterfaceServerUtils.getPayload(request);

            string domainName = requestParams.GetValue("domain").ToString();
            string userName = requestParams.GetValue("username").ToString();
            Debug.WriteLine("Domain: " + domainName);
            Debug.WriteLine("Username: " + userName);

            var responsePayload = new ResponsePayload();
            responsePayload = await ClientAuthServices.deleteUser(domainName, userName, context);
            var response = context.Response;

            WebInterfaceServerUtils.sendResponse(response, responsePayload);
        }
    }
}
