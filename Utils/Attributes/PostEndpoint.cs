using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Utils.Attributes
{
    internal class PostEndpoint: Attribute
    {
        public string value { get; set; }
        public PostEndpoint(string _value)
        {
            value = _value;
        }
    }
}
