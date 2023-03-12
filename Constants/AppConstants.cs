using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Passwordless_Authenticator.Constants
{
    internal class AppConstants
    {
        public const string GENERIC_ERROR = "Something Went Wrong Please try again later !";

        //Database
        public const string DB_NAME = "asym-auth.db";
        public const string COPY_DB_NAME = "copy_asym-auth.db";
        public const string ENC_DB_NAME = "enc_asym-auth.db";

        public static string COPY_DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, AppConstants.COPY_DB_NAME);
        public static string ENC_DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, AppConstants.ENC_DB_NAME);

    }
}
