using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials.UI;

namespace Passwordless_Authenticator.Services.HTTPServer
{
    internal class ServerListenerService
    {
        public static void SimpleListenerExample()
        {
            if (!HttpListener.IsSupported)
            {
                Debug.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            listener.Prefixes.Add("http://localhost:3000/");
            listener.Start();
            Debug.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request.
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string responseString = "{\"test\":\"hello\"}";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "Status OK";
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                var windowsAuthVerification = UserConsentVerifier.RequestVerificationAsync("Log IN!");
                output.Close();
            }
        }
    }
}
