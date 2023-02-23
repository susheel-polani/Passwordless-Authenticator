using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class Route : Attribute
    {
        public string value { get; set; }
        public Route(string _value) {
            value= _value;
        }
    }
}
