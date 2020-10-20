using Microsoft.Extensions.Configuration;

namespace StopGerry
{
    public static class GlobalConfig
    {
        public static IConfiguration Configuration;

        public static void InitConfig()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
        
    }
}