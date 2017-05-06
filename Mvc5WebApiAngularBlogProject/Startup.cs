using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mvc5WebApiAngularBlogProject.Startup))]
namespace Mvc5WebApiAngularBlogProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
