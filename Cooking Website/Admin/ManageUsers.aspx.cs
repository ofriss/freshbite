using Cooking_Website.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Cooking_Website.Admin
{
    public partial class ManageUsers : Page
    {
        private readonly string _conn = SqlHelper.LoadConnectionString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindUsers();
        }

        // ── Bind — same pattern as ViewUsers ─────────────────────

        private void BindUsers()
        {
            DataSet ds;
            try
            {
                using (var con = new SqlConnection(_conn))
                using (var da = new SqlDataAdapter(
                    "SELECT Id, Username, Birthday, Gender, Cuisine, Skill FROM Users ORDER BY Id", con))
                {
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

                html += "<tr>";
                html += $"<td>{id}</td>";
                html += $"<td>{username}</td>";
                html += $"<td>{birthday}</td>";
                html += $"<td>{gender}</td>";
                html += $"<td>{cuisine}</td>";
                html += $"<td>{skill}</td>";
                html += "<td class='action-cell'>";
                html += $"<button class='action-btn edit-btn' type='button' " +
                        $"onclick=\"openEdit({id},'{username}','{birthdayVal}','{gender}','{cuisine}','{skill}')\">Edit</button>";
                html += $"<button class='action-btn delete-btn' type='button' " +
                        $"onclick=\"confirmDelete({id},'{username}')\">Delete</button>";
                html += "</td>";
                html += "</tr>";
            }

            html += "</tbody></table>";
            adminDiv.InnerHtml = html;
        }

        // ── Create ────────────────────────────────────────────────

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            string username = Request.Form["mu-username"] ?? "";
            string password = Request.Form["mu-password"] ?? "";
            string birthday = Request.Form["mu-birthday"] ?? "";
            string gender = Request.Form["mu-gender"] ?? "";
            string cuisine = Request.Form["mu-cuisine"] ?? "";
            string skill = Request.Form["mu-skill"] ?? "";

            try
            {
                using (var con = new SqlConnection(_conn))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO Users (Username, Password, Birthday, Gender, Cuisine, Skill)
                    VALUES (@Username, @Password, @Birthday, @Gender, @Cuisine, @Skill)", con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Birthday",
                        !string.IsNullOrEmpty(birthday) ? (object)DateTime.Parse(birthday) : DBNull.Value);
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
                    cmd.Parameters.AddWithValue("@Birthday",
                        !string.IsNullOrEmpty(birthday) ? (object)DateTime.Parse(birthday) : DBNull.Value);
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

        // ── Helper ────────────────────────────────────────────────

        private void ShowMessage(string text, System.Drawing.Color color)
        {
            lblMessage.Text = text;
            lblMessage.ForeColor = color;
        }
    }
}