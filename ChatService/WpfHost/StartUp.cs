using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(WpfHost.Startup))]
namespace WpfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // הגדרת SignalR
            app.MapSignalR();
        }
    }
}
