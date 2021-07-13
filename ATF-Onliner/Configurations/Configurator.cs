using System;
using System.IO;
using System.Reflection;
using ATF_Onliner.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATF_Onliner.Services
{
    public static class Configurator
    {
        private static readonly Lazy<IConfiguration> s_configuration;

        public static IConfiguration Configuration => s_configuration.Value;
        public static string BaseUrl => Configuration[nameof(BaseUrl)];

        public static string BrowserType => Configuration[nameof(BrowserType)];
        public static string DbConnectionStrings => Configuration.GetConnectionString("DefaultConnection");

        public static int GetValue(string name)
        {
            return int.Parse(Configuration[nameof(name)]);
        }
        
        static Configurator()
        {
            s_configuration = new Lazy<IConfiguration>(BuildConfiguration);
        }

        private static IConfiguration BuildConfiguration()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json");

            var appSettingsFiles = Directory.EnumerateFiles(basePath, "appsettings.*.json");

            foreach (var appSettingsFile in appSettingsFiles)
            {
                builder.AddJsonFile(appSettingsFile);
            }

            return builder.Build();
        }

        // public void ConfigureServices(IServiceCollection services)
        // {
        //     services.AddDbContext<UserContext>(options => options.UseMySQL)
        // }
    }
}