using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProject.Util
{
    public static class ConfigurationExt
    {

        public static T GetProperty<T>(this IConfiguration configuration, string section, string prop)
        {
            IConfigurationSection configurationSection = configuration.GetSection(section);
            return configurationSection.GetValue<T>(prop);
        }

        public static T GetProperty<T>(this IConfiguration configuration, string prop)
        {
            return configuration.GetValue<T>(prop);
        }


    }
}
