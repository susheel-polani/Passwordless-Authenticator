using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Security.Cryptography;
using Passwordless_Authenticator.Services.Crypto;
using Passwordless_Authenticator.Dao_controllers;
using Passwordless_Authenticator.Services.Keys;
using Passwordless_Authenticator.Constants;
using Passwordless_Authenticator.Services.SQLite;
using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;
using Windows.System;

namespace Passwordless_Authenticator.Utils
{
    internal class DbUtils
    {
        // add funcitonality for copying the DB, during export and import
        public static void copyDB()
        {
            // modify this so that it takes user input for the destination location for exporting, and user input for from location when importing

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "asym-auth.db");
            string copy_dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "copy_asym-auth.db");

            //var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            //folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            //folderPicker.FileTypeFilter.Add("*");

            //StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            //Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

            //string enc_dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "encrypted.db");

            //tring dest_path = Path.Combine(folder.Path, "encrypted.db");

            //string password = "passwordkeyvault";
            File.Copy(dbpath, copy_dbpath, true);
            // FileEncryptionService.EncryptFile(copy_dbpath, enc_dbpath, password);
            // Debug.Write("File encrpted");
            // File.Copy(enc_dbpath, dest_path, true);

            // File.Delete(copy_dbpath);
        }

        public static void appendXML(string copy_dbpath)
        {
            DataAccess.executeQuery(copy_dbpath, DBQueries.ADD_XML, null);
            List<JObject> result = DataAccess.executeQuery(copy_dbpath, DBQueries.SELECT_ALL, null);
            foreach (JObject obj in result)
            {
                string uid = (string)obj["user_id"];
                string containerID = (string)obj["key_container_id"];

                string key2xml = RSAKeyServices.KeyToXml(containerID);

                List<SqliteParameter> parameters = new List<SqliteParameter> {
                new SqliteParameter(DBQueries.PARAM_USER_ID, uid),
                new SqliteParameter(DBQueries.PARAM_KEYXML, key2xml)
                };

                DataAccess.executeQuery(copy_dbpath, DBQueries.ADD_KEYXML, parameters);

            }
        }


    public static void populateDB()
        {
            // populates the db with dummy values. Need to set up a query to delete users

            RSAKeyServices.DeleteKeyFromContainer("container1");
            RSAKeyServices.DeleteKeyFromContainer("container2");
            RSAKeyServices.DeleteKeyFromContainer("container3");

            var rsa = RSAKeyContainerUtils.fetchContainer("container1");
            UserAuthDataController.insertUserData("facebook", "user1", "container1");
            rsa = RSAKeyContainerUtils.fetchContainer("container2");
            UserAuthDataController.insertUserData("google", "user2", "container2");
            rsa = RSAKeyContainerUtils.fetchContainer("container3");
            UserAuthDataController.insertUserData("linkedin", "user3", "container3");

        }
    }
}
