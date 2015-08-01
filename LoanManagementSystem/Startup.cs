using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoanManagementSystem.Startup))]
namespace LoanManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
