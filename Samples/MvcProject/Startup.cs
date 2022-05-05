using FCms.Auth.Abstract;
using FCms.Auth.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FCmsSample
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
            services.AddAuthentication("fcms").AddCookie("fcms", config =>
            {
                config.Cookie.Name = "FcmsAuth.Cookie";
                config.LoginPath = "/fcmsmanager/login";
                config.AccessDeniedPath = "/fcmsmanager/";
            });

            // DI area
            services.AddScoped<ICmsMember, CmsMember>();
            services.AddScoped<ICmsAuthentication, ConfigAuthentication>();
                        
            // config inject way
            var adminAuthConfig = new AdminAuthConfig();
            Configuration.GetSection("FCmsAuth").GetSection("Admin").Bind(adminAuthConfig);
            services.AddSingleton<AdminAuthConfig>(adminAuthConfig);
            
            
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
        {
            FCms.CMSConfigurator.Configure(config["DataLocation"] ?? "./");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
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
