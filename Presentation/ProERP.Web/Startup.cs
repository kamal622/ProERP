using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProERP.Web.Startup))]
namespace ProERP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
