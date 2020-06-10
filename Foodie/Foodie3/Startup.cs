using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Foodie3.Startup))]
namespace Foodie3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
