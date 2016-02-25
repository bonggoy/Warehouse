using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Warehouse.Admin.Frontend.Startup))]
namespace Warehouse.Admin.Frontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
