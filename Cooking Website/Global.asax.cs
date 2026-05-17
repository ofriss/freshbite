using System;

namespace Cooking_Website
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["Online"] = 0;
            Application["LoggedIn"] = 0;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Application["Online"] = (int)Application["Online"] + 1;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application["Online"] = (int)Application["Online"] - 1;
            if (Session["Id"] != null)
                Application["LoggedIn"] = (int)Application["LoggedIn"] - 1;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}