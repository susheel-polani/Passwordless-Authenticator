using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Constants.HTTPServer;
using Passwordless_Authenticator.Services.WebInterfaceApis;
using Passwordless_Authenticator.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.Security.Credentials.UI;

using Passwordless_Authenticator.Utils;
using Windows.Storage.Streams;

namespace Passwordless_Authenticator.Services.HTTPServer
{
    internal class WebInterfaceServer
    {
        public static async void startServer() {

            await Task.Run(() =>
            {
                if (!HttpListener.IsSupported)
                {
                    Debug.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                    return;
                }

                // Create a listener.
                HttpListener listener = new HttpListener();
                
                // Add the prefixes.
                listener.Prefixes.Add(WebInterfaceServerConstants.APP_SERVER_URL);
                listener.Start();

                Debug.WriteLine($"Server started on {WebInterfaceServerConstants.APP_SERVER_URL}");
                // Note: The GetContext method blocks while waiting for a request.
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    WebInterfaceServerUtils.enableCORS(context);
                    WebInterfaceServerUtils.invokeEndpoint(context);                   
                }
            });
        }

       

    }
}
