using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Constants.HTTPServer
{
    internal class WebInterfaceServerConstants
    {
        public const string APP_SERVER_URL = "http://localhost:4000/";
        
        public const string HTTP_METHOD_GET = "GET";
        public const string HTTP_METHOD_POST = "POST";
        public const string HTTP_METHOD_PUT = "PUT";
        public const string HTTP_METHOD_DELETE = "DELETE";
        
        public static readonly string HTTP_RESP_DESC_OK = "OK";
        public static readonly string HTTP_RESP_UNAUTHORIZED = "Unauthorized access to Key Vault";

        public const string ERR_LISTENER_NOT_SUPPORTED = "Listener Not Supported !";
        public static readonly string ERR_URL_NOT_FOUND = "URL Not found";
        public static readonly string ERR_REMOTE_ACCESS = "Access Denied. Remote Access Not Allowed";
        public static readonly string ERR_HTTP_INTERNAL_ERROR = "Internal Server Error";

        public static readonly string USER_ALREADY_EXISTS = "Username for this Domain already exists";
        public static readonly string KEY_GEN_SUCCESS = "Key Pair Successfully generated!";
        public static readonly string WINDOWS_AUTH_FAIL = "Windows Authentication Failed!";
        public static readonly string USERNAME_FETCH_SUCCESS = "List of usernames for the domain fetched successfully";
    }
}
