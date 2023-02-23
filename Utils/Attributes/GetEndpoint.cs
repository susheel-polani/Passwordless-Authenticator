using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Passwordless_Authenticator.Models;

namespace Passwordless_Authenticator.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class GetEndpoint: Attribute, EndpointAttribute
    {
        public string value { get; set; }
        public GetEndpoint(string _value)
        {
            value = _value;
        }
    }
}
