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

                // Safe for JS string literals: escape backslash then single quote
                string usernameJs = username.Replace("\\", "\\\\").Replace("'", "\\'");

                html += "<tr>";
                html += $"<td>{id}</td>";
                html += $"<td>{HttpUtility.HtmlEncode(username)}</td>";
                html += $"<td>{birthday}</td>";
                html += $"<td>{gender}</td>";
                html += $"<td>{HttpUtility.HtmlEncode(cuisine)}</td>";
                html += $"<td>{skill}</td>";
                html += "<td class='action-cell'>";
                html += $"<button class='action-btn edit-btn' type='button' " +
                        $"onclick=\"openEdit({id},'{usernameJs}','{birthdayVal}','{gender}','{cuisine}','{skill}')\">Edit</button>";
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
            string username = Request.Form["mu-username"] ?? "";
            string password = Request.Form["mu-password"] ?? "";
            string birthday = Request.Form["mu-birthday"] ?? "";
            string gender = Request.Form["mu-gender"] ?? "";
            string cuisine = Request.Form["mu-cuisine"] ?? "";
            string skill = Request.Form["mu-skill"] ?? "";

            // Server-side validation (mirror of Register) — stop on first error
            string validationMessage = ValidateForm(username, password, birthday, gender, cuisine, skill, isCreate: true, userId: 0);
            if (validationMessage != null)
            {
                ShowMessage(validationMessage, System.Drawing.Color.Red);
                BindUsers();
                return;
            }

            try
            {
                using (var con = new SqlConnection(_conn))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO Users (Username, Password, Birthday, Gender, Cuisine, Skill)
                    VALUES (@Username, @Password, @Birthday, @Gender, @Cuisine, @Skill)", con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    // Fall back to DateTime.MinValue when the birthday can't be parsed (matches Register)
                    if (!DateTime.TryParse(birthday, out DateTime bday))
                        bday = DateTime.MinValue;
                    cmd.Parameters.AddWithValue("@Birthday", bday);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Cuisine", cuisine);
                    cmd.Parameters.AddWithValue("@Skill", skill);
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
            string username = Request.Form["mu-username"] ?? "";
            string password = Request.Form["mu-password"] ?? "";
            string birthday = Request.Form["mu-birthday"] ?? "";
            string gender = Request.Form["mu-gender"] ?? "";
            string cuisine = Request.Form["mu-cuisine"] ?? "";
            string skill = Request.Form["mu-skill"] ?? "";

            if (!int.TryParse(idStr, out int userId) || userId == 0)
            {
                ShowMessage("Invalid user.", System.Drawing.Color.Red);
                BindUsers();
                return;
            }

            // Server-side validation (mirror of Register) — stop on first error
            string validationMessage = ValidateForm(username, password, birthday, gender, cuisine, skill, isCreate: false, userId: userId);
            if (validationMessage != null)
            {
                ShowMessage(validationMessage, System.Drawing.Color.Red);
                BindUsers();
                return;
            }

            try
            {
                // Leave password out of the query if blank — keep existing
                string sql = string.IsNullOrWhiteSpace(password)
                    ? @"UPDATE Users SET Username=@Username, Birthday=@Birthday,
                              Gender=@Gender, Cuisine=@Cuisine, Skill=@Skill WHERE Id=@Id"
                    : @"UPDATE Users SET Username=@Username, Password=@Password,
                            Birthday=@Birthday, Gender=@Gender, Cuisine=@Cuisine,
                            Skill=@Skill WHERE Id=@Id";

                using (var con = new SqlConnection(_conn))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    cmd.Parameters.AddWithValue("@Username", username);
                    // Fall back to DateTime.MinValue when the birthday can't be parsed (matches Register)
                    if (!DateTime.TryParse(birthday, out DateTime bday))
                        bday = DateTime.MinValue;
                    cmd.Parameters.AddWithValue("@Birthday", bday);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Cuisine", cuisine);
                    cmd.Parameters.AddWithValue("@Skill", skill);

                    if (!string.IsNullOrWhiteSpace(password))
                        cmd.Parameters.AddWithValue("@Password", password);

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

                // Password doesn't contain special characters except !*@_$#
                if (Regex.IsMatch(password, @"[^a-zA-Z0-9!*@_$#]"))
                    return "Password can't have special characters (other than !*@_$#).";
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

        // ── Helper ────────────────────────────────────────────────

        // Sets the result label text and colour in one call
        private void ShowMessage(string text, System.Drawing.Color color)
        {
            lblMessage.Text = text;
            lblMessage.ForeColor = color;
        }
    }
}