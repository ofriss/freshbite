using Cooking_Website.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cooking_Website
{
    public partial class Profile : System.Web.UI.Page
    {
        protected Dictionary<string, object> profileInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            profileInfo = SqlHelper.GetRow((int)Session["Id"]);
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> form = new Dictionary<string, object>();
            form.Add("Username", Request.Form["Username"].ToString());
            if (DateTime.TryParse(Request.Form["Birthday"], out DateTime birthday))
                form.Add("Birthday", birthday);
            form.Add("Gender", Request.Form["Gender"].ToString().ToLower());
            form.Add("Cuisine", Request.Form["Cuisine"].ToString().ToLower().Split(','));
            form.Add("Skill", Request.Form["Skill"].ToString().ToLower());

            Dictionary<string, object> dataToChange = CompareToProfile(form);

            string currentPassword = Request.Form["CurrentPassword"]?.ToString() ?? "";
            string newPassword = Request.Form["NewPassword"]?.ToString() ?? "";
            if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword) && !currentPassword.Equals(newPassword))
            {
                if (SqlHelper.CheckCredentials(Session["Username"].ToString(), currentPassword))
                {
                    dataToChange.Add("Password", newPassword);
                }
                else
                {
                    serverMsg.Text = "Wrong password, please try again.";
                    serverMsg.Style["display"] = "block";
                    serverMsg.CssClass = "server-message server-message-error";
                    return;
                }
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
                profileInfo = SqlHelper.GetRow((int)Session["Id"]);

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
                    (!profileInfo.ContainsKey(kvp.Key) ||
                    !ValuesAreEqual(kvp.Value, profileInfo[kvp.Key])))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        // Special helper function for comparison of objects
        private bool ValuesAreEqual(object a, object b)
        {
            // For string[]
            if (a is string[] arrA && b is string[] arrB)
                return arrA.SequenceEqual(arrB);

            // Everything else
            return Equals(a, b);
        }

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