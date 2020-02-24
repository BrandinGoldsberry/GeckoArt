using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectGecko
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(s => s.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc
            (
                m =>
                {
                    m.MapRoute("UserFeed", "{userid:int}", new { controller = "User", action = "ShowFeed"});
                    m.MapRoute("UserAccount", "{userid:int}/account", new { controller = "User", action = "ShowAccount" });
                    m.MapRoute("PostArt", "{userid:int}/NewPost", new { controller = "Post", action = "CreatePost" });
                    m.MapRoute("ShowPost", "{postid:int}/ShowPost", new { controller = "Post", action = "ShowPost"})
                    m.MapRoute("CreateAccount", "createaccount", new { controller = "User", action = "CreateAccount" });
                    m.MapRoute("Home", "/", new { controller = "Home", action = "Index"});
                    m.MapRoute("Catchall", "{**a}", new { controller = "Home", action = "Index" });
                }
            );
        }
    }
}
