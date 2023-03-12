using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Data.Sqlite;
using Passwordless_Authenticator.Constants;
using System.Diagnostics;
using Passwordless_Authenticator.Utils;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Reflection.PortableExecutable;

namespace Passwordless_Authenticator.Services.SQLite
{
    internal class DataAccess
    {
        public async static Task<bool> setUpDatabase() {
            try {
                await ApplicationData.Current.LocalFolder.CreateFileAsync(AppConstants.DB_NAME, CreationCollisionOption.OpenIfExists);
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, AppConstants.DB_NAME);

                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath};"))
                {
                   
                    db.Open();
                    List<string> queries = new List<string>() {
                        DBQueries.CREATE_DOMAINS_TABLE,
                        DBQueries.CREATE_USER_AUTH_PREF_TABLE,
                        DBQueries.CREATE_USER_DATA_TABLE,
                    };

                    foreach (string query in queries) {
                       new SqliteCommand(query, db).ExecuteReader();
                    }
                    
                }


                return true;
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return false;
            }
           
        }
        
        public static List<JObject> executeQuery(string dbpath, string query, List<SqliteParameter> parameters) {
            
            List<JObject> result = new List<JObject>();
            
            //string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, AppConstants.DB_NAME);
            
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                using ( SqliteCommand command = new SqliteCommand())
                {
                
                    command.Connection = db;

                    command.CommandText = query;

                    if (parameters != null)
                    {
                        foreach (SqliteParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }


                    using (SqliteDataReader dataReader = command.ExecuteReader())
                    {


                            if (!dataReader.HasRows)
                            {
                                return result;
                            }

                            var schemaTable = dataReader.GetSchemaTable();

                            // Get the column names from the schema table
                            var columns = schemaTable.Rows
                                .OfType<DataRow>()
                                .Select(row => row["ColumnName"].ToString())
                                .ToList();



                            while (dataReader.Read())
                            {
                                JObject obj = new JObject();
                                foreach (var column in columns)
                                {
                                    var value = dataReader[column];
                                    Debug.WriteLine($">>> DB : column = {column} : value : {value}");
                                    // JObject obj = new JObject();
                                    obj.Add(new JProperty(column, value));
                                    // result.Add(obj);
                                }
                                result.Add(obj);
                            }
                    }


                }

                db.Close();
            }

            GC.Collect();
            Debug.WriteLine(">>>>>> DB : Query executed");
            return result;
        }
        
    }
}
