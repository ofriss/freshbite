using Cooking_Website.DAL;
using Cooking_Website.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Cooking_Website.Admin
{
    public partial class ManageUsers : Page
    {
        private readonly string _conn = SqlHelper.LoadConnectionString();

        // On first load only — subsequent postbacks are handled by the individual button click handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindUsers();
        }

        // ── Bind — same pattern as ViewUsers ─────────────────────

        // Renders the full users table as raw HTML including inline onclick calls for edit/delete
        private void BindUsers()
        {
            DataSet ds;
            try
            {
                using (var conn = new SqlConnection(_conn))
                using (var da = new SqlDataAdapter(
                    "SELECT Id, Username, Birthday, Gender, Cuisine, Skill FROM Users ORDER BY Id", conn))
                {
                    conn.Open();
                    ds = new DataSet();
                    da.Fill(ds);
                }
            }
            catch
            {
                errorMsg.Text = "Something went wrong while reading the database.";
                return;
            }

            string html = "<table class='users-table'>";
            html += "<thead><tr>";
            html += "<th>ID</th><th>Username</th><th>Birthday</th>";
            html += "<th>Gender</th><th>Cuisine</th><th>Skill</th><th>Actions</th>";
            html += "</tr></thead><tbody>";

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = Convert.ToInt32(row["Id"]);
                string username = row["Username"].ToString();
                string birthday = row["Birthday"] != DBNull.Value
                    ? Convert.ToDateTime(row["Birthday"]).ToString("dd MMM yyyy")
                    : "—";
                string birthdayVal = row["Birthday"] != DBNull.Value
                    ? Convert.ToDateTime(row["Birthday"]).ToString("yyyy-MM-dd")
                    : "";
                string gender = row["Gender"].ToString();
                string cuisine = row["Cuisine"].ToString();
                string skill = row["Skill"].ToString();

                // JS-escape all string fields placed inside single-quoted onclick arguments
                string usernameJs = username.Replace("\\", "\\\\").Replace("'", "\\'");
                string genderJs = gender.Replace("\\", "\\\\").Replace("'", "\\'");
                string cuisineJs = cuisine.Replace("\\", "\\\\").Replace("'", "\\'");
                string skillJs = skill.Replace("\\", "\\\\").Replace("'", "\\'");

                html += "<tr>";
                html += $"<td>{id}</td>";
                html += $"<td>{HttpUtility.HtmlEncode(username)}</td>";
                html += $"<td>{birthday}</td>";
                html += $"<td>{HttpUtility.HtmlEncode(gender)}</td>";
                html += $"<td>{HttpUtility.HtmlEncode(cuisine)}</td>";
                html += $"<td>{HttpUtility.HtmlEncode(skill)}</td>";
                html += "<td class='action-cell'>";
                html += $"<button class='action-btn edit-btn' type='button' " +
                        $"onclick=\"openEdit({id},'{usernameJs}','{birthdayVal}','{genderJs}','{cuisineJs}','{skillJs}')\">Edit</button>";
                html += $"<button class='action-btn delete-btn' type='button' " +
                        $"onclick=\"confirmDelete({id},'{usernameJs}')\">Delete</button>";
                html += "</td>";
                html += "</tr>";
            }

            html += "</tbody></table>";
            adminDiv.InnerHtml = html;
        }

        // ── Create ────────────────────────────────────────────────

        // Reads form values written by manage-users.js, validates, then inserts a new user
        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            var f = ReadForm();

            // Validate (shows the message + refreshes the table itself on failure)
            if (!TryValidate(f, isCreate: true, userId: 0))
                return;

            try
            {
                using (var con = new SqlConnection(_conn))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO Users (Username, Password, Birthday, Gender, Cuisine, Skill)
                    VALUES (@Username, @Password, @Birthday, @Gender, @Cuisine, @Skill)", con))
                {
                    cmd.Parameters.AddWithValue("@Username", f.Username);
                    cmd.Parameters.AddWithValue("@Password", f.Password);
                    cmd.Parameters.AddWithValue("@Birthday", DateTime.Parse(f.Birthday));
                    cmd.Parameters.AddWithValue("@Gender", f.Gender);
                    cmd.Parameters.AddWithValue("@Cuisine", f.Cuisine);
                    cmd.Parameters.AddWithValue("@Skill", f.Skill);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                ShowMessage("User created successfully.", System.Drawing.Color.Green);
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, System.Drawing.Color.Red);
            }

            BindUsers();
        }

        // ── Edit ──────────────────────────────────────────────────

        // Updates an existing user; password column is omitted from UPDATE when the field is blank
        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            string idStr = Request.Form["mu-id"] ?? "0";
            var f = ReadForm();

            if (!int.TryParse(idStr, out int userId) || userId == 0)
            {
                ShowMessage("Invalid user.", System.Drawing.Color.Red);
                BindUsers();
                return;
            }

            // Validate (shows the message + refreshes the table itself on failure)
            if (!TryValidate(f, isCreate: false, userId: userId))
                return;

            try
            {
                // Leave password out of the query if blank — keep existing
                string sql = string.IsNullOrWhiteSpace(f.Password)
                    ? @"UPDATE Users SET Username=@Username, Birthday=@Birthday,
                              Gender=@Gender, Cuisine=@Cuisine, Skill=@Skill WHERE Id=@Id"
                    : @"UPDATE Users SET Username=@Username, Password=@Password,
                            Birthday=@Birthday, Gender=@Gender, Cuisine=@Cuisine,
                            Skill=@Skill WHERE Id=@Id";

                using (var con = new SqlConnection(_conn))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    cmd.Parameters.AddWithValue("@Username", f.Username);
                    cmd.Parameters.AddWithValue("@Birthday", DateTime.Parse(f.Birthday));
                    cmd.Parameters.AddWithValue("@Gender", f.Gender);
                    cmd.Parameters.AddWithValue("@Cuisine", f.Cuisine);
                    cmd.Parameters.AddWithValue("@Skill", f.Skill);

                    if (!string.IsNullOrWhiteSpace(f.Password))
                        cmd.Parameters.AddWithValue("@Password", f.Password);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                ShowMessage("User updated successfully.", System.Drawing.Color.Green);
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, System.Drawing.Color.Red);
            }

            BindUsers();
        }

        // ── Delete ────────────────────────────────────────────────

        // Deletes the user by ID passed from the JS confirmation panel via hidden field
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string idStr = Request.Form["mu-id"] ?? "0";

            if (!int.TryParse(idStr, out int userId) || userId == 0)
            {
                ShowMessage("Invalid user.", System.Drawing.Color.Red);
                BindUsers();
                return;
            }

            try
            {
                using (var con = new SqlConnection(_conn))
                using (var cmd = new SqlCommand(
                    "DELETE FROM Users WHERE Id = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                ShowMessage("User deleted.", System.Drawing.Color.DarkRed);
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, System.Drawing.Color.Red);
            }

            BindUsers();
        }

        // ── Validation ────────────────────────────────────────────

        // Validate the submitted user fields (mirror of Register.ValidateForm).
        // In edit mode a blank password means "keep existing", so password rules
        // are only enforced when a password was supplied. Returns null when valid.
        private string ValidateForm(string username, string password, string birthday,
            string gender, string cuisine, string skill, bool isCreate, int userId)
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

            // Duplicate username — exclude the current user when editing
            bool exists = isCreate
                ? UsersRepository.DoesUserExist(username)
                : UsersRepository.DoesUserExist(username, userId);
            if (exists)
                return "User with the same username already exists. Please try a different one.";

            // Password — in edit mode a blank password keeps the existing one, so skip its rules
            bool validatePassword = isCreate || !string.IsNullOrWhiteSpace(password);
            if (validatePassword)
            {
                if (string.IsNullOrWhiteSpace(password))
                    return "Password is missing.";

                password = password.Trim();

                if (password.Length < 8)
                    return "Password is too short.";

                if (password.Contains(" "))
                    return "Password can't have spaces.";

                if (!password.Any(char.IsLower) || !password.Any(char.IsUpper))
                    return "Password must have upper and lower letters.";

                if (!password.Any(char.IsDigit))
                    return "Password must have at least one digit.";

                // Password doesn't contain special characters except !*@_$#
                if (Regex.IsMatch(password, @"[^a-zA-Z0-9!*@_$#]"))
                    return "Password can't have special characters (other than !*@_$#).";

                if (DateTime.TryParse(birthday, out DateTime bdayParsed) && password.Contains(bdayParsed.Year.ToString()))
                    return "Password can't have the birthday year in it.";

                if (password.Contains(username))
                    return "Password can't have the username in it.";
            }

            // Birthday
            if (string.IsNullOrWhiteSpace(birthday))
                return "Birthday is missing.";

            if (!DateTime.TryParse(birthday, out DateTime bday))
                return "Birthday is invalid.";

            int age = DateTime.Today.Year - bday.Year;
            // Check if the birthday did not happen yet this year, then decrement age
            if (bday.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 12 || age > 120)
                return "Age must be between 12 and 120.";

            // Gender
            if (string.IsNullOrWhiteSpace(gender))
                return "Gender is missing.";

            if (gender != "male" && gender != "female")
                return "Gender is invalid.";

            // Cuisine
            if (string.IsNullOrWhiteSpace(cuisine))
                return "Cuisine is missing.";

            var allowedCuisines = new HashSet<string> { "italian", "indian", "japanese", "mediterranean", "mexican", "american", "chinese", "french", "other" };
            var submitted = cuisine.Split(',').Select(c => c.Trim().ToLower()).ToList();

            if (!submitted.All(c => allowedCuisines.Contains(c)))
                return "One or more selected cuisines are invalid.";

            for (int i = 0; i < submitted.Count; i++)
            {
                for (int j = i + 1; j < submitted.Count; j++)
                {
                    // Duplicate found
                    if (submitted[i] == submitted[j])
                        return "Two or more cuisines are duplicates.";
                }
            }

            // Skill
            if (string.IsNullOrWhiteSpace(skill))
                return "Skill level is missing.";

            if (skill != "beginner" && skill != "intermediate" && skill != "expert")
                return "Skill level is invalid.";

            return null;
        }

        // ── Helpers ───────────────────────────────────────────────

        // Holds the six user fields submitted by manage-users.js via hidden inputs
        private class UserForm
        {
            public string Username;
            public string Password;
            public string Birthday;
            public string Gender;
            public string Cuisine;
            public string Skill;
        }

        // Reads the mu-* hidden fields written by manage-users.js (used by Create and Edit)
        private UserForm ReadForm()
        {
            return new UserForm
            {
                Username = Request.Form["mu-username"] ?? "",
                Password = Request.Form["mu-password"] ?? "",
                Birthday = Request.Form["mu-birthday"] ?? "",
                Gender = Request.Form["mu-gender"] ?? "",
                Cuisine = Request.Form["mu-cuisine"] ?? "",
                Skill = Request.Form["mu-skill"] ?? ""
            };
        }

        // Runs server-side validation guarded by try-catch (so a DB error inside the
        // DoesUserExist duplicate check shows a friendly message). On any problem it
        // shows the message, refreshes the table, and returns false; true only when valid.
        private bool TryValidate(UserForm f, bool isCreate, int userId)
        {
            string validationMessage;
            try
            {
                validationMessage = ValidateForm(f.Username, f.Password, f.Birthday,
                    f.Gender, f.Cuisine, f.Skill, isCreate, userId);
            }
            catch
            {
                ShowMessage("Something went wrong while validating. Please try again.", System.Drawing.Color.Red);
                BindUsers();
                return false;
            }

            if (validationMessage != null)
            {
                ShowMessage(validationMessage, System.Drawing.Color.Red);
                BindUsers();
                return false;
            }

            return true;
        }

        // Sets the result label text and colour in one call
        private void ShowMessage(string text, System.Drawing.Color color)
        {
            lblMessage.Text = text;
            lblMessage.ForeColor = color;
        }
    }
}