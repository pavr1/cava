using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cava.Startup))]
namespace cava
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
