using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspenWiki.Startup))]
namespace AspenWiki
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
