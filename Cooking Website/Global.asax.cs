using System;

namespace Cooking_Website
{
    public class Global : System.Web.HttpApplication
    {

        // Initialise application-wide counters to 0 on first startup
        protected void Application_Start(object sender, EventArgs e)
        {
            Application["Online"] = 0;
            Application["LoggedIn"] = 0;
        }

        // Track every new visitor session, whether logged in or not
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

        // Decrement online count; only decrement LoggedIn if the session had an authenticated user
        protected void Session_End(object sender, EventArgs e)
        {
            Application["Online"] = Math.Max(0, (int)Application["Online"] - 1);
            if (Session["Id"] != null)
                Application["LoggedIn"] = Math.Max(0, (int)Application["LoggedIn"] - 1);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}