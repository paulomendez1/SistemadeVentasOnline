using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemadeVentasOnline.Startup))]

namespace SistemadeVentasOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}