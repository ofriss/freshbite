using Cooking_Website.DAL;
using Cooking_Website.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cooking_Website
{
    public partial class Register : System.Web.UI.Page
    {
        string username, password, birthday, gender, cuisine, skill, terms;
        string client_msg;
        // On postback: validates, inserts user, sets session, increments LoggedIn, then redirects
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

        // Checks for duplicate username, inserts the new user, and immediately logs them in
        private bool InsertUserIntoDatabase()
        {
            try
            {
                if (UsersRepository.DoesUserExist(username))
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
            // Username
            if (string.IsNullOrWhiteSpace(username))
                return "Username is missing.";

            username = username.Trim();

            if (username.Length < 4)
                return "Username is too short.";

            if (username.Contains(" "))
                return "Username can't have spaces.";

            if (Regex.IsMatch(username, @"[^a-zA-Z0-9]"))
                return "Username can't have special characters.";

            // Password
            if (string.IsNullOrWhiteSpace(password))
                return "Password is missing.";

            password = password.Trim();

            // Password length at least 8
            if (password.Length < 8)
                return "Password is too short.";

            // Password doesn't contain any spaces
            if (password.Contains(" "))
                return "Password can't have spaces.";

            // Password contains at least one lower and upper chars
            if (!password.Any(char.IsLower) || !password.Any(char.IsUpper))
                return "Password must have upper and lower letters.";

            // Password doesn't contain special characters except !*@_$#
            if (Regex.IsMatch(password, @"[^a-zA-Z0-9!*@_$#]"))
                return "Password can't have special characters (other than !*@_$#).";

            // Birthday
            if (string.IsNullOrWhiteSpace(birthday))
                return "Birthday is missing.";

            // Birthday is valid
            if (!DateTime.TryParse(birthday, out DateTime bday))
                return "Birthday is invalid.";

            int age = DateTime.Today.Year - bday.Year;
            // bday.Date strips the hour essentially.
            // Check if the birthday did not happen yet, then decrement age
            if (bday.Date > DateTime.Today.AddYears(-age)) age--;

            // Check age in limits
            if (age < 12 || age > 120)
                return "Age must be between 12 and 120.";

            // Gender
            if (string.IsNullOrWhiteSpace(gender))
                return "Gender is missing.";

            // Limit gender options
            if (gender != "male" && gender != "female")
                return "Gender is invalid.";

            // Cuisine
            if (string.IsNullOrWhiteSpace(cuisine))
                return "Cuisine is missing.";

            // All possible cuisines
            var allowedCuisines = new HashSet<string> { "italian", "indian", "japanese", "mediterranean", "mexican", "american", "chinese", "french", "other" };
            // Parse submitted cuisines
            var submitted = cuisine.Split(',').Select(c => c.Trim().ToLower()).ToList();

            // Check if any cuisine submitted is not in the allowed cuisines
            if (!submitted.All(c => allowedCuisines.Contains(c)))
                return "One or more selected cuisines are invalid.";

            for (int i = 0; i < submitted.Count; i++)
            {
                for (int j = i + 1; j < submitted.Count; j++)
                {
                    // Duplicate found
                    if (submitted[i] == submitted[j])
                    {
                        return "Two or more cuisines are duplicates.";
                    }
                }
            }

            // Skill
            if (string.IsNullOrWhiteSpace(skill))
                return "Skill level is missing.";

            // Limit skill options
            if (skill != "beginner" && skill != "intermediate" && skill != "expert")
                return "Skill level is invalid.";

            if (string.IsNullOrWhiteSpace(terms) || terms != "yes")
                return "You must agree to the terms of use.";

            return null;
        }
    }
}