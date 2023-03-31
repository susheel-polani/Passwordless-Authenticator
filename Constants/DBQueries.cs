using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passwordless_Authenticator.Constants
{
    internal class DBQueries
    {
        // DB SETUP QUERIES

        public const string CREATE_DOMAINS_TABLE = @"
            CREATE TABLE IF NOT EXISTS `domains` (
                domain_id uuid PRIMARY KEY,
                domain_name varchar(255) UNIQUE NOT NULL
            );
        ";


        public const string CREATE_USER_AUTH_PREF_TABLE = @"
            CREATE TABLE IF NOT EXISTS `user_auth_preference` (
              id INTEGER PRIMARY KEY AUTOINCREMENT,
              auth_type Enum,
              password varchar(255)
            );
        ";

        public const string CREATE_USER_DATA_TABLE = @"
            CREATE TABLE IF NOT EXISTS `user_data` (
              domain_id uuid REFERENCES domains(domain_id),
              user_id uuid PRIMARY KEY,
              username varchar(255),
              key_container_id varchar(255) UNIQUE
            );";

        // params
        public const string PARAM_DOMAIN_ID = "@domainI";
        public const string PARAM_DOMAIN_NAME = "@domainName";
        public const string PARAM_USER_ID = "@userId";
        public const string PARAM_USER_NAME = "@username";
        public const string PARAM_KEY_CONTAINER_ID = "@keyContainerId";
        public const string PARAM_KEYXML = "@keyxml";


        // Business logic queries
        public const string SELECT_DOMAIN = $"SELECT domain_id FROM domains WHERE domain_name={PARAM_DOMAIN_NAME}";

        public const string SELECT_DOMAIN_USER = $"SELECT user_id FROM user_data WHERE domain_id={PARAM_DOMAIN_ID} and username={PARAM_USER_NAME}";

        public const string INSERT_DOMAIN = $"INSERT INTO domains (domain_id, domain_name) VALUES({PARAM_DOMAIN_ID}, {PARAM_DOMAIN_NAME})";

        public const string INSERT_USER_DATA = @$"INSERT INTO 
                                                user_data (domain_id, user_id, username, key_container_id) 
                                                VALUES ({PARAM_DOMAIN_ID}, {PARAM_USER_ID}, {PARAM_USER_NAME}, {PARAM_KEY_CONTAINER_ID} )";

        public const string GET_PUBLIC_KEY_ID = @$"SELECT key_container_id FROM user_data 
                                                WHERE domain_id = (SELECT domain_id FROM domains WHERE domain_name = {PARAM_DOMAIN_NAME}) 
                                                AND username={PARAM_USER_NAME}";

        public const string GET_USERNAMES = @$"SELECT username FROM user_data 
                                                WHERE domain_id = (SELECT domain_id FROM domains WHERE domain_name = {PARAM_DOMAIN_NAME})";

        public const string ADD_XML = $"ALTER TABLE user_data ADD COLUMN keyxml VARCHAR(256)";

        // public const string GET_ALL_USERIDS = $"SELECT user_id FROM user_data";

        // public const string GET_CONTAINER_ID = $"SELECT key_container_id FROM user_data WHERE user_id = {PARAM_USER_ID}";

        public const string SELECT_ALL = $"SELECT * from user_data";

        public const string ADD_KEYXML = $"UPDATE user_data SET keyxml = {PARAM_KEYXML} WHERE user_id = {PARAM_USER_ID}";

        public const string GET_USER_DOM =  $"SELECT username,domain_name FROM user_data LEFT JOIN domains ON domains.domain_id = user_data.domain_id";

        public const string GET_IMPORT_DATA = $"SELECT username,domain_name,key_container_id,keyxml FROM user_data LEFT JOIN domains ON domains.domain_id = user_data.domain_id";






    }
}
