using LDAPAuthentication.Constants;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace LDAPAuthentication.App_Start
{
    public partial class Startup
	{
        private const string LoginPath = "/Login";
        private const string CookieName = "CookieName";

        public void AuthenticationConfiguration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
				AuthenticationType = AUTHENTICATION.ApplicationCookie,
                LoginPath = new PathString(LoginPath),
				Provider = new CookieAuthenticationProvider(),
				CookieName = CookieName,
				CookieHttpOnly = true,
				ExpireTimeSpan = TimeSpan.FromHours(12)
            });
        }
    }
}