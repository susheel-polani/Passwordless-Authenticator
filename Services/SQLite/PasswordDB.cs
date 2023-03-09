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
    internal class PasswordDB
    {
        public async static Task<bool> InitializePwdDatabase()
        {
            try
            {
                await ApplicationData.Current.LocalFolder.CreateFileAsync("password.db", CreationCollisionOption.OpenIfExists);
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "password.db");
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    String tableCommand = "CREATE TABLE IF NOT " +
                        "EXISTS PwdTable (Primary_Key INTEGER PRIMARY KEY, " +
                        "Password NVARCHAR(2048) NOT NULL, " + "Recovery_Key NVARCHAR(2048) NOT NULL)";

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

        public static void AddPassword(string inputText, string inputText2)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "password.db");
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
                    insertCommand.CommandText = "INSERT INTO PwdTable VALUES (Null, @Entry, @Entry2);";
                    insertCommand.Parameters.AddWithValue("@Entry", inputText);
                    insertCommand.Parameters.AddWithValue("@Entry2", inputText2);
                }
                else
                {
                    insertCommand.CommandText = "UPDATE PwdTable SET Password = @Entry, Recovery_Key = @Entry2 where Primary_Key = 1;";
                    insertCommand.Parameters.AddWithValue("@Entry", inputText);
                    insertCommand.Parameters.AddWithValue("@Entry2", inputText2);
                }

                insertCommand.ExecuteReader();
            }

        }
        public static string GetPassword()
        { 
            string entries = "Empty";

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "password.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                selectCommand.CommandText = "SELECT Password from PwdTable where Primary_Key = 1";
                SqliteDataReader query = selectCommand.ExecuteReader();

                while(query.Read())
                {
                    entries = query.GetString(0);
                }

                return entries;
            }
        }

        public static string GetKey()
        {
            string entries = "Empty";

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "password.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                selectCommand.CommandText = "SELECT Recovery_Key from PwdTable where Primary_Key = 1";
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries = query.GetString(0);
                }

                return entries;
            }
        }

        public static string CheckEmpty()
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "password.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand checkCommand = new SqliteCommand();
                checkCommand.Connection = db;

                checkCommand.CommandText = "SELECT count(*) FROM PwdTable;";
                SqliteDataReader query = checkCommand.ExecuteReader();

                string entries = "Value";

                while (query.Read())
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
