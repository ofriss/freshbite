using Cooking_Website.DAL;
using Cooking_Website.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Cooking_Website
{
    public partial class Profile : System.Web.UI.Page
    {
        protected Dictionary<string, object> profileInfo;
        // Redirects unauthenticated users; loads current profile data used by the markup
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            profileInfo = UsersRepository.GetRow((int)Session["Id"]);
            if (profileInfo.Count == 0)
            {
                // GetRow returns empty dict on DB error — nothing to show, send user home
                Response.Redirect("/Index.aspx");
                return;
            }
        }

        // Server-side save handler: validates all fields, checks username uniqueness,
        // verifies current password if a change is requested, then persists only changed fields
        protected void saveBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> form = new Dictionary<string, object>();

            // Username
            string username = Request.Form["Username"]?.ToString().Trim() ?? "";
            if (username.Length < 4)
            {
                serverMsg.Text = "Username must be at least 4 characters.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            if (username.Contains(" "))
            {
                serverMsg.Text = "Username can't have spaces.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            if (Regex.IsMatch(username, @"[^a-zA-Z0-9]"))
            {
                serverMsg.Text = "Username can't have special characters.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            if (!username.Equals(profileInfo["Username"].ToString(), StringComparison.OrdinalIgnoreCase) &&
                UsersRepository.DoesUserExist(username, (int)Session["Id"]))
            {
                serverMsg.Text = "That username is already taken. Please choose a different one.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            form.Add("Username", username);

            // Birthday
            if (!DateTime.TryParse(Request.Form["Birthday"], out DateTime birthday))
            {
                serverMsg.Text = "Invalid birthday.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            int age = DateTime.Today.Year - birthday.Year;
            if (birthday.Date > DateTime.Today.AddYears(-age)) age--;
            if (age < 12 || age > 120)
            {
                serverMsg.Text = "Age must be between 12 and 120.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            form.Add("Birthday", birthday);

            // Gender
            string gender = Request.Form["Gender"]?.ToString().ToLower();
            if (gender != "male" && gender != "female")
            {
                serverMsg.Text = "Invalid gender value.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            form.Add("Gender", gender);

            // Cuisine
            string cuisineRaw = Request.Form["Cuisine"]?.ToString();
            if (string.IsNullOrEmpty(cuisineRaw))
            {
                serverMsg.Text = "Please select at least one cuisine.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            var allowedCuisines = new HashSet<string> { "italian", "indian", "japanese", "mediterranean", "mexican", "american", "chinese", "french", "other" };
            string[] cuisineArr = cuisineRaw.ToLower().Split(',');
            if (!cuisineArr.All(c => allowedCuisines.Contains(c.Trim())))
            {
                serverMsg.Text = "One or more selected cuisines are invalid.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            form.Add("Cuisine", cuisineArr);

            // Skill
            string skill = Request.Form["Skill"]?.ToString().ToLower();
            if (skill != "beginner" && skill != "intermediate" && skill != "expert")
            {
                serverMsg.Text = "Invalid skill level.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
                return;
            }
            form.Add("Skill", skill);

            Dictionary<string, object> dataToChange = CompareToProfile(form);

            string currentPassword = Request.Form["CurrentPassword"]?.ToString() ?? "";
            string newPassword = Request.Form["NewPassword"]?.ToString() ?? "";
            if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword))
            {
                if (!UsersRepository.CheckCredentials(Session["Username"].ToString(), currentPassword))
                {
                    serverMsg.Text = "Wrong current password, please try again.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                if (currentPassword.Equals(newPassword))
                {
                    serverMsg.Text = "New password must be different from current password.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                if (newPassword.Length < 8)
                {
                    serverMsg.Text = "New password must be at least 8 characters.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                if (newPassword.Contains(" "))
                {
                    serverMsg.Text = "New password cannot contain spaces.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                if (!newPassword.Any(char.IsLower) || !newPassword.Any(char.IsUpper))
                {
                    serverMsg.Text = "New password must have upper and lower case letters.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                if (!newPassword.Any(char.IsDigit))
                {
                    serverMsg.Text = "New password must have at least one digit.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                if (Regex.IsMatch(newPassword, @"[^a-zA-Z0-9!*@_$#]"))
                {
                    serverMsg.Text = "Password can't have special characters (other than !*@_$#).";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                if (newPassword.Contains(birthday.Year.ToString()))
                {
                    serverMsg.Text = "New password can't have your birthday year in it.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                if (newPassword.Contains(username))
                {
                    serverMsg.Text = "New password can't have your username in it.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }

                dataToChange.Add("Password", newPassword);
            }

            if (dataToChange.Count == 0)
                return;

            if (!Update(dataToChange))
            {
                serverMsg.Text = "Oops! Something went wrong, please try again later.";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-error";
            }
            else
            {
                serverMsg.Text = "Update successful";
                serverMsg.Style["display"] = "block";
                serverMsg.CssClass = "server-message server-message-info";

                // Reload data
                profileInfo = UsersRepository.GetRow((int)Session["Id"]);

                // Update username if changed
                if (dataToChange.ContainsKey("Username"))
                    Session["Username"] = dataToChange["Username"];
            }
        }

        // Function gets a dictionary and checks if the values there are different than the ones stored in sql
        private Dictionary<string, object> CompareToProfile(Dictionary<string, object> dict)
        {
            return dict.Where(kvp =>
                    !string.IsNullOrEmpty(kvp.Value?.ToString()) &&
                    !ValuesAreEqual(kvp.Value, profileInfo[kvp.Key]))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        // Special helper function for comparison of objects by value
        private bool ValuesAreEqual(object a, object b)
        {
            // For string[]
            if (a is string[] arrA && b is string[] arrB)
                return arrA.SequenceEqual(arrB);

            // Everything else
            return Equals(a, b);
        }

        // Builds a dynamic UPDATE statement from only the keys in fields; string[] values are joined with commas
        private bool Update(Dictionary<string, object> fields)
        {
            try
            {
                string conStr = SqlHelper.LoadConnectionString();

                using (SqlConnection con = new SqlConnection(conStr))
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    string setClauses = string.Join(", ", fields.Keys.Select(k => $"{k} = @{k}"));
                    string query = $"UPDATE Users Set {setClauses} WHERE Id = @Id";

                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@Id", Session["Id"]);
                    foreach (var kvp in fields)
                    {
                        var value = kvp.Value is string[] arr ? string.Join(",", arr) : kvp.Value;
                        cmd.Parameters.AddWithValue(kvp.Key, value);
                    }

                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}