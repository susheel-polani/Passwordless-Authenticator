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
              username varchar(255) UNIQUE,
              key_container_id varchar(255) UNIQUE
            );";

        // params
        public const string PARAM_DOMAIN_ID = "@domainI";
        public const string PARAM_DOMAIN_NAME = "@domainName";
        public const string PARAM_USER_ID = "@userId";
        public const string PARAM_USER_NAME = "@username";
        public const string PARAM_KEY_CONTAINER_ID = "@keyContainerId";


        // Business logic queries
        public const string INSERT_DOMAIN = $"INSERT INTO domains (domain_id, domain_name) VALUES({PARAM_DOMAIN_ID}, {PARAM_DOMAIN_NAME})";

        public const string INSERT_USER_DATA = @$"INSERT INTO 
                                                user_data (domain_id, user_id, username, key_container_id) 
                                                VALUES ({PARAM_DOMAIN_ID}, {PARAM_USER_ID}, {PARAM_USER_NAME}, {PARAM_KEY_CONTAINER_ID} )";

        public const string GET_PUBLIC_KEY_ID = @$"SELECT key_container_id FROM user_data 
                                                WHERE domain_id = (SELECT domain_id FROM domains WHERE domain_name = {PARAM_DOMAIN_NAME}) 
                                                AND username={PARAM_USER_NAME}";






    }
}
