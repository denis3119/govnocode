using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(govnocode.Startup))]
namespace govnocode
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
