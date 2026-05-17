using Cooking_Website.Security;
using System;

namespace Cooking_Website
{
    public partial class Login : System.Web.UI.Page
    {
        string username, password;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                return;

            username = Request.Form["uname"];
            password = Request.Form["pwd"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Please enter both username and password.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (SqlHelper.CheckCredentials(username, password, out int id))
            {
                Session["Id"] = id;
                Session["Username"] = username;
                Response.Redirect("/Index.aspx");
                lblMessage.Text = $"Login successful for {username}";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMessage.Text = $"Invalid username or password.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }


    }
}