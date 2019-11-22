using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KC_FinancialPortal.Startup))]
namespace KC_FinancialPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
