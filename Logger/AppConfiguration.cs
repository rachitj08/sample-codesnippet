using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger
{
    public static class AppConfiguration
    {
        public static readonly string _commonConnectionString = string.Empty;
        static AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            if (root.GetSection("ConnectionStrings") == null)
            {
                throw new KeyNotFoundException("ConnectionStrings section not found.");
            }

            if (root.GetSection("ConnectionStrings").GetSection("DefaultConnection") == null)
            {
                throw new KeyNotFoundException("DefaultConnection section not found.");
            }

            _commonConnectionString = root.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }
        public static string CommonConnectionString
        {
            get => _commonConnectionString;
        }
    }
}
