using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;


namespace FCms.Auth
{
    public class ConfigAuthentication: ICmsAuthentication
    {

        IConfiguration config;

        public ConfigAuthentication()
        {
            this.config = (IConfiguration)ServiceCollection.GetRequiredService<IConfiguration>();
        }

        public bool Authenticate(string username, string password)
        {
            return username == config.GetValue<string>("FCmsAuth:Admin:Username") && password == config.GetValue<string>("FCmsAuth:Admin:Password");
        }
    }
}
