using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Studfile.Startup))]
namespace Studfile
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
