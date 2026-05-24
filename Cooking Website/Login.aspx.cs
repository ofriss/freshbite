using Cooking_Website.Security;
using System;
using System.Linq;
using System.Text.RegularExpressions;

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

            string validationMessage = ValidateForm();
            if (validationMessage != null)
            {
                lblMessage.Text = validationMessage;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (SqlHelper.CheckCredentials(username, password, out int id))
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

        private string ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Username is missing.";

            if (username.Length < 6)
                return "Username is too short.";

            if (username.Contains(" "))
                return "Username can't have spaces.";

            if (Regex.IsMatch(username, @"[^a-zA-Z0-9]"))
                return "Username can't have special characters.";

            if (string.IsNullOrWhiteSpace(password))
                return "Password is missing.";

            if (password.Length < 8)
                return "Password is too short.";

            if (password.Contains(" "))
                return "Password can't have spaces.";

            if (!password.Any(char.IsLower) || !password.Any(char.IsUpper))
                return "Password must have upper and lower letters.";

            if (Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
                return "Password can't have special characters.";

            username = username.Trim();
            password = password.Trim();

            return null;
        }
    }
}