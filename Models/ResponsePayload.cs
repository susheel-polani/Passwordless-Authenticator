using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Models
{
    internal class ResponsePayload
    {
        public bool isSuccess { set; get; }
        public string message { set; get; }

        public JObject payload { set; get; }
    }
}
