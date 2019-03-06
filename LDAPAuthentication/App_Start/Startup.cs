using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LDAPAuthentication.App_Start.Startup))]

namespace LDAPAuthentication.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AuthenticationConfiguration(app);
        }
    }
}
