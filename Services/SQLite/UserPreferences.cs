using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Diagnostics;

namespace Passwordless_Authenticator.Services.SQLite
{
    internal class UserPrefDB
    {
        public async static void InitializeUsrPrfDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("preferences.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "preferences.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS UsrPrfTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Setting_Entry NVARCHAR(2048) NOT NULL, " + "Value_Entry NVARCHAR(2048) NOT NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddPassword(string inputText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "password.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO PwdTable VALUES (Null, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();
            }

        }
        public static string GetPref()
        { 
            string entries = "Empty";

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "preferences.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                selectCommand.CommandText = "SELECT Value_Entry from UsrPrfTable where Setting_Entry = 'AuthPreference'";

                SqliteDataReader query = selectCommand.ExecuteReader();

                while(query.Read())
                {
                    entries = query.GetString(0);
                }

                return entries;
            }
        }

        // TBD
        public static void DropTable()
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passwordless-KeyPairs.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "DROP TABLE IF EXISTS MyTable;";

                SqliteCommand dropTable = new SqliteCommand(tableCommand, db);

                dropTable.ExecuteReader();
            }
        }
    }
}
