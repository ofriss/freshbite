using System;

namespace Cooking_Website
{
    public partial class AdminSidebar : System.Web.UI.MasterPage
    {
        // Restricts the entire Admin section to the hard-coded admin account
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["Username"] != "ofri")
            {
                Response.Redirect("/Index.aspx");
            }
        }
    }
}