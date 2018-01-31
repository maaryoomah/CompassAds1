using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CompassAds1.Startup))]
namespace CompassAds1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
