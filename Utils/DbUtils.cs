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
using Passwordless_Authenticator.Constants;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Controls;
using Windows.Security.Authentication.OnlineId;

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

        public static async Task<string> importDB()
        {
            try
            {
                List<JObject> userdom_result = DataAccess.executeQuery(AppConstants.DB_PATH, DBQueries.GET_USER_DOM, null);

                var existing_entries = new List<string>();

                foreach (JObject obj in userdom_result)
                {
                    string username = (string)obj["username"];
                    string domain_name = (string)obj["domain_name"];
                    existing_entries.Add(username + domain_name);
                }

                List<JObject> import_result = DataAccess.executeQuery(AppConstants.IMP_DB_PATH, DBQueries.GET_IMPORT_DATA, null);


                var rsa1 = RSAKeyContainerUtils.fetchContainer("container1");
                Debug.WriteLine("Before overwriting: " + rsa1.ToXmlString(true));

                foreach (JObject obj in import_result)
                {
                    string username = (string)obj["username"];
                    string domain_name = (string)obj["domain_name"];
                    string container_name = (string)obj["key_container_id"];
                    string xml_string = (string)obj["keyxml"];

                    string identifier = username + domain_name;

                    if (existing_entries.Contains(identifier))
                    {
                        Debug.Write("Entry already exists");
                        bool result = await PromptUserdataOverwrite(username, domain_name);
                        Debug.WriteLine(" Result is : " + result);
                        if (result)
                        {
                            var rsa = RSAKeyContainerUtils.fetchContainer(container_name);
                            rsa.FromXmlString(xml_string);

                        }
                        else
                        {

                        }
                    }

                    else
                    {
                        var rsa = RSAKeyContainerUtils.fetchContainer(container_name);
                        rsa.FromXmlString(xml_string);
                        UserAuthDataController.insertUserData(domain_name, username, container_name);

                    }

                }



                rsa1 = RSAKeyContainerUtils.fetchContainer("container1");
                Debug.WriteLine("After overwriting: " + rsa1.ToXmlString(true));

                return "Database imported successfully.";
            }

            catch (Exception ex)
            {
                Debug.Write(ex);
                return "Database import failed." ;
            }


        }

        public static async Task<bool> PromptUserdataOverwrite(string username, string domain_name)
        {
            ContentDialog overwriteUsernameDialog = new ContentDialog
            {
                Title = "User already exists.",
                Content = "This system already has the username: " + username + "for the domain: " + domain_name + ". Do you want to overwrite it with the new data?",
                PrimaryButtonText = "Overwrite",
                CloseButtonText = "Do not overwrite"
            };

            overwriteUsernameDialog.XamlRoot = App.m_window.Content.XamlRoot;

            ContentDialogResult result = await overwriteUsernameDialog.ShowAsync();

            // Delete the file if the user clicked the primary button.
            /// Otherwise, do nothing.
            if (result == ContentDialogResult.Primary)
            {
                // Delete the file.
                return true;
            }
            else
            {
                return false;
            }
        }


        public static void populateDB()
        {
            // populates the db with dummy values. Need to set up a query to delete users
            RSAKeyServices.DeleteKeyFromContainer("container1");
            RSAKeyServices.DeleteKeyFromContainer("container4");
            RSAKeyServices.DeleteKeyFromContainer("container5");
            RSAKeyServices.DeleteKeyFromContainer("container6");
            RSAKeyServices.DeleteKeyFromContainer("container7");

            var rsa = RSAKeyContainerUtils.fetchContainer("container4");
            UserAuthDataController.insertUserData("facebook", "user4", "container4");
            rsa = RSAKeyContainerUtils.fetchContainer("container5");
            UserAuthDataController.insertUserData("google", "user5", "container5");
            rsa = RSAKeyContainerUtils.fetchContainer("container6");
            UserAuthDataController.insertUserData("amazon", "user6", "container6");
            rsa = RSAKeyContainerUtils.fetchContainer("container7");
            UserAuthDataController.insertUserData("amazon", "user7", "container7");
            rsa = RSAKeyContainerUtils.fetchContainer("container1");
            UserAuthDataController.insertUserData("facebook", "user1", "container1");


        }
    }
}
