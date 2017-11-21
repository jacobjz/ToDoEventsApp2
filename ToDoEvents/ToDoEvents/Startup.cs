using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToDoEvents.Startup))]
namespace ToDoEvents
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
