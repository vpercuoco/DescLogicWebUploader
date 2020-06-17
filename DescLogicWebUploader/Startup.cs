using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstASPNETCOREProject;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FirstASPNETCOREProject
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //you have controllers that you want to result with views
            services.AddControllersWithViews();

            //Apparently the command below will register my Desclogic Service with the service layer, using an interface as a middleman?
            services.AddTransient<IDescLogicService, DescLogicService>();

            services.AddHostedService<FileDeletorService>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            //If I only want to serve static files, then UseDefaultFiles
           // app.UseDefaultFiles();
            app.UseStaticFiles();

            //I installed OdeToNode from Nuget to get this command.
            
            app.UseNodeModules();

            //Routing allows your classes to map to views
            app.UseRouting();


            //This code is autogenerated on project creation, but it is the code that connects the model to a view
              app.UseEndpoints(endpoints =>
              {
                  endpoints.MapControllerRoute("Fallback", "{controller}/{action}/{id?}", new { controller = "App", action = "Index" });

                 /* endpoints.MapGet("/", async context =>
                  {
                      //  await context.Response.WriteAsync("Hello World");                     
                  });*/
              });
              
        }
    }
}