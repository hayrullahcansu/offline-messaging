using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace OfflineMessaging.Utils
{
    public class AppConfig
    {
        private static AppConfig _instance;

        internal const int CONFIG_CACHE_MINUTES = 10;
        internal static IConfigurationRoot _configurationRoot;
        private static DateTime _ConfigurationLoadTime = DateTime.MinValue;

        private AppConfig()
        {
        }


        public static AppConfig Instance
        {
            get
            {
                if (_instance == null || _ConfigurationLoadTime < DateTime.UtcNow)
                {
                    try
                    {
                        _configurationRoot = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();
                    }
                    catch (Exception ex)
                    {
                        _configurationRoot = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory() + "\\..\\/")
                            .AddJsonFile("appsettings.json")
                            .Build();
                    }

                    _instance = new AppConfig();
                    _instance.AppSettings = _configurationRoot.GetSection("AppSettings").Get<AppSettings>();
                    _ConfigurationLoadTime = DateTime.UtcNow.AddMinutes(CONFIG_CACHE_MINUTES);
                }

                return _instance;
            }
        }

        public AppSettings AppSettings { get; set; }
    }

    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireDays { get; set; }
    }
}