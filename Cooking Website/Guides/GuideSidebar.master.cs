using System;

namespace Cooking_Website
{
    public partial class GuideSidebar : System.Web.UI.MasterPage
    {
        // Layout-only master; no server-side logic on load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] == null)
                Response.Redirect("/Login.aspx");
        }
    }
}