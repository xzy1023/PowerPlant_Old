using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerPlant.Models.Context;
using PowerPlant.Service;
using PowerPlant.Tools.PropertyMapping;
using Web_Material_Calculator.Configs;

namespace Web_Material_Calculator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();  // Razor page runtime change compilation

            services.AddAutoMapper(typeof(AutoMapperProfile));      // Auto mapper: convention-based object-object mapper

            // services.AddDatabaseDeveloperPageExceptionFilter();     // captures database-related exceptions

            services.AddControllersWithViews();

            #region Model Injection
            string connection = Configuration.GetValue<string>("ConnectionStrings Selection");  // Get ConnectionStrings Selection Name from "appsettings.json"
            services.AddDbContext<PPDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(connection)));   // Add DbContext

            // Add model services
            services.AddTransient<DbContext, PPDbContext>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IFacilityRepository, FacilityRepository>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            #endregion
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
