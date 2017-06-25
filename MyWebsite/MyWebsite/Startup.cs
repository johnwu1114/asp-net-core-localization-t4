using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Resources;

namespace MyWebsite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<ILocalizer, Localizer>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{culture=en-GB}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}