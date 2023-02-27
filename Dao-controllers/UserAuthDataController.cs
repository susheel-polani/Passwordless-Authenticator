using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Constants;
using Passwordless_Authenticator.Services.SQLite;
using Passwordless_Authenticator.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Dao_controllers
{
    internal class UserAuthDataController
    {
        public static void test() {
            // string domainId = insertDomain("whatsapp.facebook.com");
            //insertUserAuthData(domainId, AppUtils.getUUID(), "mani", "xzc");
           // List<JObject> result = getPublicKey("whatsapp.facebook.com", "mani");

            // Debug.WriteLine(result[0].GetValue("key_container_id"));

        }

        public static List<JObject> insertUserData(string domainName, string username, string keyContainerId)        {

            string domainId = insertDomain("whatsapp.facebook.com");
            List<JObject> result = insertUserAuthData(domainId, AppUtils.getUUID(), "mani", "xzc");
            return result;
        }
        public static string insertDomain(string domainName) {
            string domainId = AppUtils.getUUID();
            List < SqliteParameter > parameters= new List<SqliteParameter> {
                new SqliteParameter(DBQueries.PARAM_DOMAIN_ID, domainId),
                new SqliteParameter(DBQueries.PARAM_DOMAIN_NAME, domainName)
            };
            DataAccess.executeQuery(DBQueries.INSERT_DOMAIN, parameters);
            return domainId; 
        }

        public static List<JObject> insertUserAuthData(
            string domainId, 
            string userId, 
            string username,
            string keyContainerId) {

            List<SqliteParameter> parameters = new List<SqliteParameter> { 
                new SqliteParameter(DBQueries.PARAM_DOMAIN_ID, domainId),
                new SqliteParameter(DBQueries.PARAM_USER_ID, userId),
                new SqliteParameter(DBQueries.PARAM_USER_NAME, username),
                new SqliteParameter(DBQueries.PARAM_KEY_CONTAINER_ID, keyContainerId)
            };

            List<JObject> result = DataAccess.executeQuery(DBQueries.INSERT_USER_DATA, parameters);

            return result;
        }

        public static List<JObject> getPublicKeyContainerId(string domainName, string username) {

            List<SqliteParameter> parameters = new List<SqliteParameter> { 
                new SqliteParameter(DBQueries.PARAM_DOMAIN_NAME, domainName),
                new SqliteParameter(DBQueries.PARAM_USER_NAME, username),
            };

            List<JObject> result = DataAccess.executeQuery(DBQueries.GET_PUBLIC_KEY_ID, parameters);

            return result;
        }
    }
}
