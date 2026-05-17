using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cooking_Website.Security
{
    public class SqlHelper
    {
        private static bool isLoaded = false;
        private static string connStr;

        // Get connection string from Web.config
        public static string LoadConnectionString()
        {
            if (isLoaded) return connStr;
            connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            // Throw an exception if connection string is missing in Web.config
            if (string.IsNullOrEmpty(connStr))
                throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
            isLoaded = true;
            return connStr;
        }

        public static bool DoesUserExist(string username)
        {
            try
            {
                string connStr = LoadConnectionString();

                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Users WHERE Username = @Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<string, object> GetRow(int id)
        {
            try
            {
                using (var con = new SqlConnection(LoadConnectionString()))
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = $@"SELECT * FROM dbo.Users WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("Username", reader["Username"].ToString());
                            dict.Add("Password", reader["Password"].ToString());
                            dict.Add("Birthday", reader["Birthday"]);
                            dict.Add("Gender", reader["Gender"].ToString());
                            dict.Add("Cuisine", reader["Cuisine"].ToString().Split(','));
                            dict.Add("Skill", reader["Skill"].ToString());

                            return dict;
                        }
                        else
                        {
                            throw new Exception($"User not found (ID = {id})");
                        }
                    }
                }
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }

        public static bool CheckCredentials(string username, string password) => CheckCredentials(username, password, out _);

        public static bool CheckCredentials(string username, string password, out int id)
        {
            try
            {
                string connStr = LoadConnectionString();

                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    const string sql = @"
                    SELECT Id
                    FROM dbo.Users
                    WHERE Username=@Username AND Password=@Password";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;

                        // Parameters
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        id = (int)(cmd.ExecuteScalar() ?? 0);
                        if (id == 0) return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                id = 0;
                return false;
            }
        }
    }
}