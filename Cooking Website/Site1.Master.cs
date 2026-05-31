using Cooking_Website.Helpers;
using System;
using System.Data.SqlClient;

namespace Cooking_Website
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        // Decrements the LoggedIn counter only when a logged-in session is active, then clears and abandons the session
        private void Logout()
        {
            if (Session["Id"] != null)
                Application["LoggedIn"] = (int)Application["LoggedIn"] - 1;
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Index.aspx");
        }

        // Logout button in the global nav
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        // Permanently deletes the logged-in user's account then logs out
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string connStr = SqlHelper.LoadConnectionString();
            const string query = "DELETE FROM Users WHERE Id=@Id";

            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", Session["Id"]);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            Logout();
        }
    }
}