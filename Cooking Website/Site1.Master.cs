using Cooking_Website.Security;
using System;
using System.Data.SqlClient;

namespace Cooking_Website
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private void Logout()
        {
            if (Session["Id"] != null)
                Application["LoggedIn"] = (int)Application["LoggedIn"] - 1;
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Index.aspx");
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

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