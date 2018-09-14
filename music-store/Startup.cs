using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(music_store.Startup))]
namespace music_store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
