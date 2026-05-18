using Cooking_Website.Security;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Cooking_Website.Admin
{
    public partial class ViewUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connStr = SqlHelper.LoadConnectionString();

            // Load data to 
            DataSet ds;
            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var da = new SqlDataAdapter("SELECT * FROM Users", conn))
                {
                    conn.Open();

                    ds = new DataSet();
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                errorMsg.Text = "Something went wrong while reading db.";
                return;
            }

            string str = "<table class='users-table'>";
            str += "<tr>";
            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                if (column.ColumnName == "Password")
                {
                    continue;
                }
                str += $"<th>{column.ColumnName}</th>";
            }
            str += "</tr>";
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                str += "<tr>";
                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    if (column.ColumnName == "Password")
                    {
                        continue;
                    }
                    str += $"<td>{row[column]}</td>";
                }
                str += "</tr>";
            }
            str += "</table>";
            adminDiv.InnerHtml = str;
        }
    }
}