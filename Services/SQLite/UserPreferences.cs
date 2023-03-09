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
        public async static Task<bool> InitializeUsrPrfDatabase()
        {
            try
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string CheckEmpty()
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "preferences.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand checkCommand = new SqliteCommand();
                checkCommand.Connection = db;

                checkCommand.CommandText = "SELECT count(*) FROM UsrPrfTable;";
                SqliteDataReader query = checkCommand.ExecuteReader();

                string entries = "Value";

                while (query.Read())
                {
                    entries = query.GetString(0);

                }

                return entries;
            }
        }

        public static void SetPref(string inputText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "preferences.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                string entries = CheckEmpty();
                    
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                if (entries == "0")
                {
                    insertCommand.CommandText = "INSERT INTO UsrPrfTable VALUES (NULL, 'AuthPreference', @Entry);";
                    insertCommand.Parameters.AddWithValue("@Entry", inputText);
                }
                else
                {
                    insertCommand.CommandText = "UPDATE UsrPrfTable SET Value_Entry = @Entry where Setting_Entry = 'AuthPreference'";
                    insertCommand.Parameters.AddWithValue("@Entry", inputText);
                }

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
                Debug.WriteLine(entries);

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
