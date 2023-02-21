using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Models
{
    internal class WindowsAuthData
    {
        public bool flag;
        public string message;
        public WindowsAuthData(bool _flag, string _message)
        {
            this.flag = _flag;
            this.message = _message;
        }
    }
}
