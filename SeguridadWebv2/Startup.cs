using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SeguridadWebv2.Startup))]
namespace SeguridadWebv2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}