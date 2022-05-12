using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse
{
    class Appsetting
    {
        public IConfigurationRoot config { get; set; }
        public Appsetting()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsetting.json", optional: true, reloadOnChange: true);
            config = builder.Build();
        }
        public string ConfigValue(string Key)
        {
            return config[Key];
        }
    }
}
