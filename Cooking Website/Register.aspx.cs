using Cooking_Website.Security;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cooking_Website
{
    public partial class Register : System.Web.UI.Page
    {
        string username, password, birthday, gender, cuisine, skill, terms;
        string client_msg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                return;

            username = Request.Form["uname"];
            password = Request.Form["pwd"];
            birthday = Request.Form["bday"];
            gender = Request.Form["gender"];
            cuisine = Request.Form["cuisine"];
            skill = Request.Form["skill"];
            terms = Request.Form["terms"];

            string validationMessage = ValidateForm();

            // Is input valid?
            if (validationMessage != null)
            {
                lblMessage.Text = validationMessage;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (InsertUserIntoDatabase())
            {
                lblMessage.Text = $"Registration successful for {username}";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            // Special message?
            else if (client_msg != "")
            {
                lblMessage.Text = client_msg;
                client_msg = "";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
            // Default error message
            else
            {
                lblMessage.Text = "Registration failed. Please try again later.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private bool InsertUserIntoDatabase()
        {
            try
            {
                if (SqlHelper.DoesUserExist(username))
                {
                    client_msg = "User with the same username already exists. Please try a different one.";
                    return false;
                }

                string connStr = SqlHelper.LoadConnectionString();

                // Insert user to db
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    const string sql = @"
                    INSERT INTO [Users] (Username, Password, Birthday, Gender, Cuisine, Skill)
                    OUTPUT INSERTED.Id
                    VALUES (@username, @password, @birthday, @gender, @cuisine, @skill);";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;

                        // Parameters
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        if (!DateTime.TryParse(birthday, out DateTime bday))
                            bday = DateTime.MinValue;
                        cmd.Parameters.AddWithValue("@birthday", bday);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@cuisine", cuisine);
                        cmd.Parameters.AddWithValue("@skill", skill);

                        int newId = (int)cmd.ExecuteScalar();
                        Session["Id"] = newId;
                        Session["Username"] = username;

                        Application["LoggedIn"] = (int)Application["LoggedIn"] + 1;

                        Response.Redirect("/Index.aspx");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Validate form to make sure user provides correct values for registration
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

            if (string.IsNullOrWhiteSpace(birthday))
                return "Birthday is missing.";

            if (!DateTime.TryParse(birthday, out _))
                return "Birthday is invalid.";

            if (string.IsNullOrWhiteSpace(gender))
                return "Gender is missing.";

            if (gender != "male" && gender != "female")
                return "Gender is invalid.";

            if (string.IsNullOrWhiteSpace(cuisine))
                return "Cuisine is missing.";

            if (string.IsNullOrWhiteSpace(skill))
                return "Skill level is missing.";

            if (skill != "beginner" && skill != "intermediate" && skill != "advanced")
                return "Skill level is invalid.";

            if (string.IsNullOrWhiteSpace(terms) || terms != "yes")
                return "You must agree to the terms of use.";

            username = username.Trim();
            password = password.Trim();

            return null;
        }
    }
}