using Cooking_Website.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Cooking_Website.DAL
{
    // DAL for user lookup and credential checking; all methods are static (no instance state)
    public class UsersRepository
    {
        // Returns true when a user with the given username already exists
        public static bool DoesUserExist(string username)
        {
            string connStr = SqlHelper.LoadConnectionString();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Users WHERE Username = @Username", conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // Same as the overload above, but ignores a given user's Id
        // (used when editing a user so its own username isn't flagged as a duplicate).
        public static bool DoesUserExist(string username, int excludeId)
        {
            string connStr = SqlHelper.LoadConnectionString();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Users WHERE Username = @Username AND Id <> @ExcludeId", conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@ExcludeId", excludeId);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // Fetches a user's profile fields as a dictionary; returns an empty dict if not found or on error
        public static Dictionary<string, object> GetRow(int id)
        {
            try
            {
                using (var con = new SqlConnection(SqlHelper.LoadConnectionString()))
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
                            return new Dictionary<string, object>();
                        }
                    }
                }
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }

        // Convenience overload when the caller doesn't need the user ID
        public static bool CheckCredentials(string username, string password) => CheckCredentials(username, password, out _);

        // Validates username/password and returns the matching user ID via out parameter; returns false on mismatch or error
        public static bool CheckCredentials(string username, string password, out int id)
        {
            try
            {
                string connStr = SqlHelper.LoadConnectionString();

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