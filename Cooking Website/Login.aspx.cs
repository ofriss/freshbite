using Cooking_Website.DAL;
using System;

namespace Cooking_Website
{
    public partial class Login : System.Web.UI.Page
    {
        string username, password;

        // On postback: validates credentials, sets session vars, increments LoggedIn counter, then redirects
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                return;

            username = Request.Form["uname"];
            password = Request.Form["pwd"];

            string validationMessage = ValidateForm();
            if (validationMessage != null)
            {
                lblMessage.Text = validationMessage;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (UsersRepository.CheckCredentials(username, password, out int id))
            {
                Session["Id"] = id;
                Session["Username"] = username;

                Application["LoggedIn"] = (int)Application["LoggedIn"] + 1;

                Response.Redirect("/Index.aspx");
            }
            else
            {
                lblMessage.Text = $"Invalid username or password.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        // Server-side presence check only; strength rules are enforced on the client
        private string ValidateForm()
        {
            username = username?.Trim();
            password = password?.Trim();

            if (string.IsNullOrWhiteSpace(username))
                return "Username is missing.";

            if (string.IsNullOrWhiteSpace(password))
                return "Password is missing.";

            return null;
        }
    }
}