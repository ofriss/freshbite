using System;
using System.Configuration;

namespace Cooking_Website.Helpers
{
    // Provides a lazily-loaded, cached connection string; read once from Web.config on first call
    public class SqlHelper
    {
        private static bool isLoaded = false;
        private static string connStr;

        // Returns the DefaultConnection string; caches it in a static field after the first read
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
    }
}