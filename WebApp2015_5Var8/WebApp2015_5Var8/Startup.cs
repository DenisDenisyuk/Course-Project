using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApp2015_5Var8.Startup))]
namespace WebApp2015_5Var8
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
