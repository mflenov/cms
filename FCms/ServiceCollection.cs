using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace FCms
{
    public static class ServiceCollection
    {
        private static IApplicationBuilder app; 


        public static void Configure(IApplicationBuilder appBuilder)
        {
            app = appBuilder;

            HttpContext.Configure(app?.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
        }

        public static object GetRequiredService<T>()
        {
            if (app != null)
                return app.ApplicationServices.GetRequiredService<T>();
            return null;
        }
    }
}
