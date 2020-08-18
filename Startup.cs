using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EkasiBuilders.Startup))]
namespace EkasiBuilders
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
