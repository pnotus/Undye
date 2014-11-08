using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Undye.Web.Startup))]
namespace Undye.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
