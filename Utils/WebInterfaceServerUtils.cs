using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Passwordless_Authenticator.Utils.Attributes;
using Passwordless_Authenticator.Services.WebInterfaceApis;
using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Constants.HTTPServer;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Specialized;
using Passwordless_Authenticator.Models;
using Windows.Storage.Streams;

namespace Passwordless_Authenticator.Utils
{
    internal class WebInterfaceServerUtils
    {
        /// <summary>
        /// This method executes the corresponding end-points
        /// </summary>
        /// <param name="context"></param>
        public static void invokeEndpoint(HttpListenerContext context) {
            HttpListenerRequest request = context.Request;

            NameValueCollection endPointInfo = getEndpointInfo(request.Url);

            Type classType = getRouteClass(endPointInfo.Get("route"));

            if (!request.IsLocal) {
                sendRemoteAccessDeniedResponse(context.Response);
                return;
            }

            if (classType == null)
            {
                sendUrlNotFoundResponse(context.Response);
                return;
            }

            var method = getMethod(classType, endPointInfo.Get("endpoint"), request.HttpMethod);

            if (method == null)
            {
                sendUrlNotFoundResponse(context.Response);
                return;
            }

            method.Invoke(Activator.CreateInstance(classType), new object[] { context });

        }

        /// <summary>
        /// This method returns class type of the endpoint requested
        /// </summary>
        /// <param name="route"></param>
        /// <returns><typeparam>Type - class Type</typeparam></returns>
        public static Type getRouteClass(string route) {
            try {
                Type routeClass = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.GetCustomAttribute<Route>()?.value == route).First();
                return routeClass;
            } catch (Exception e) { 
                Debug.WriteLine(e);
                return null;
            }
          
        }

        /// <summary>
        /// This method returns MethodInfo of the enpoint method requested
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="endpoint"></param>
        /// <param name="methodType"></param>
        /// <returns> MethodInfo </returns>
        public static MethodInfo getMethod(Type classType, string endpoint, string methodType) {            
            

            try
            {
                
                switch (methodType) {
                    case WebInterfaceServerConstants.HTTP_METHOD_GET:
                         return classType.GetTypeInfo().GetMethods()
                           .Where(m => m.GetCustomAttributes(typeof(GetEndpoint), true)
                           .Cast<EndpointAttribute>()
                           .Any(attr => attr.value == endpoint)).First();
                        

                    case WebInterfaceServerConstants.HTTP_METHOD_POST: 
                        return classType.GetTypeInfo().GetMethods()
                           .Where(m => m.GetCustomAttributes(typeof(PostEndpoint), true)
                           .Cast<PostEndpoint>()
                           .Any(attr => attr.value == endpoint)).First();


                    case WebInterfaceServerConstants.HTTP_METHOD_PUT:
                        return classType.GetTypeInfo().GetMethods()
                           .Where(m => m.GetCustomAttributes(typeof(PutEndpoint), true)
                           .Cast<PutEndpoint>()
                           .Any(attr => attr.value == endpoint)).First();

                    case WebInterfaceServerConstants.HTTP_METHOD_DELETE:
                        return classType.GetTypeInfo().GetMethods()
                           .Where(m => m.GetCustomAttributes(typeof(PutEndpoint), true)
                           .Cast<PutEndpoint>()
                           .Any(attr => attr.value == endpoint)).First();




                    default: return null;
                } 
       
            }
            catch (Exception e){ 
                Debug.WriteLine(e);
                return null; 
            }
        }

        /// <summary>
        /// This method parses the query params and the data from request body to in the form of JObject(Json)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>JObject</returns>
        public static JObject getPayload(HttpListenerRequest request)
        {
            if (request.HttpMethod == WebInterfaceServerConstants.HTTP_METHOD_GET)
            {
                var queryString = request.QueryString;
                var dictionary = queryString.AllKeys.ToDictionary(k => k, k => queryString[k]);
                return JObject.FromObject(dictionary);
            }

            if (request.HasEntityBody)
            {
                var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                var payloadString = reader.ReadToEnd();
                return JObject.Parse(payloadString);
            }

            return new JObject();
        }

        /// <summary>
        /// This method parses the URL of the request and generates Route and endpoint url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>NameValueCollection</returns>
        public static NameValueCollection getEndpointInfo(Uri uri) { 
            string[] segments = uri.Segments;
            NameValueCollection endPointInfo = new NameValueCollection();

            if (segments.Length == 1) {
                
                endPointInfo.Add("route","/" );
                endPointInfo.Add("endpoint", "");

                return endPointInfo;
                
            }

            string endPoint = "/" + segments[segments.Length - 1];
            var routeSegments = segments.SkipLast(1).ToArray();
            string route = String.Join("", routeSegments);
           

            endPointInfo.Add("route", route.Remove(route.Length - 1, 1));
            endPointInfo.Add("endpoint", endPoint.Remove(endPoint.Length - 1, 1));

            return endPointInfo;

        }

      
        /// <summary>
        /// This method sends response back to the client
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responsePayload"></param>
        public static void sendResponse(HttpListenerResponse response, ResponsePayload responsePayload) {

            JObject responsePayloadJobject = JObject.FromObject(responsePayload);
            string dataString = responsePayloadJobject.ToString();

            byte[] buffer = Encoding.UTF8.GetBytes(dataString);
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        /// <summary>
        /// This method sends 404 error as the requested url is not available
        /// </summary>
        /// <param name="response"></param>
        public static void sendUrlNotFoundResponse(HttpListenerResponse response) {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.StatusDescription = WebInterfaceServerConstants.ERR_URL_NOT_FOUND;

            var responsePayload = new ResponsePayload();
            responsePayload.isSuccess = false;
            responsePayload.message = WebInterfaceServerConstants.ERR_URL_NOT_FOUND;
            responsePayload.payload = new JObject();

            sendResponse(response, responsePayload);
        }
        /// <summary>
        /// This method sends Forbidden error if the request orginated from external device
        /// </summary>
        /// <param name="response"></param>
        public static void sendRemoteAccessDeniedResponse (HttpListenerResponse response) {

            response.StatusCode = (int)HttpStatusCode.Forbidden;
            response.StatusDescription = WebInterfaceServerConstants.ERR_REMOTE_ACCESS;

            var responsePayload = new ResponsePayload();
            responsePayload.isSuccess = false;
            responsePayload.message = WebInterfaceServerConstants.ERR_URL_NOT_FOUND;
            responsePayload.payload = new JObject();

            sendResponse(response, responsePayload);
        }


    }
}
