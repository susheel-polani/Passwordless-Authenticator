using Passwordless_Authenticator.Constants;
using Passwordless_Authenticator.Services.Crypto;
using Passwordless_Authenticator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Passwordless_Authenticator.Services.Keys
{
    internal class KeyUtils
    {
        // add export and import key functionality here
        public static async void exportKeys()
        {
            DbUtils.copyDB();
            DbUtils.appendXML(AppConstants.COPY_DB_PATH);
            FileEncryptionService.EncryptFile(AppConstants.COPY_DB_PATH, AppConstants.ENC_DB_PATH, "passwordkeyvault");
        }
    }
}
