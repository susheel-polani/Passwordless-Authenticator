using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Constants
{
    internal class DBQueries
    {
        public const string CREATE_ASYM_AUTH_TABLE = @"
                    CREATE TABLE IF NOT 
                    EXISTS asym_auth (
                        Primary_Key INTEGER PRIMARY KEY,
                        Text_Entry NVARCHAR(2048) NULL
                    )";
    }
}
