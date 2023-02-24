using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class DeleteEndpoint: Attribute
    {
        public string value { get; set; }
        public DeleteEndpoint(string _value)
        {
            value = _value;
        }
    }
}
