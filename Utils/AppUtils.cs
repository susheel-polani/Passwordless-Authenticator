using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Utils
{
    internal class AppUtils
    {
        public static string getUUID() {
            Guid guid= Guid.NewGuid();
            return guid.ToString();
        }
    }
}
