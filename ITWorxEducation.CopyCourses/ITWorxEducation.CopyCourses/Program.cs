using ITWorxEducation.CopyCourses.ApplicationBootStrap;
using ITWorxEducation.CopyCourses.Helpers;
using ITWorxEducation.CopyCourses.Helpers.JsonConverterManagment;
using ITWorxEducation.CopyCourses.IServices;
using ITWorxEducation.CopyCourses.IServices.IMaterial;
using ITWorxEducation.CopyCourses.Services;
using ITWorxEducation.CopyCourses.Services.Material;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace ITWorxEducation.CopyCourses
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = createHostBuilder(args).Build();
            host.Services.GetRequiredService<ToolStart>().Run();
        }

        
        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            return builder.Build();
        }

        private static IHostBuilder createHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => {
                    services.AddSingleton<ToolStart>();
                    services.AddHttpClient("githubClient", c => c.BaseAddress = new Uri("https://api.github.com"));
                    services.AddTransient<IWinjiCourses,WinjiCourses>();
                    services.AddScoped<IOrganizationManegment, OrganizationManegment>();
                    services.AddScoped<ICallBackRestApi, CallBackRestApi>();
                    services.AddScoped<JsonManagment>();
                    services.AddScoped<IWinjiRounds,WinjiRounds>();
                    services.AddScoped<IWinjiUnit, WinjiUnit>();
                    services.AddScoped<IWinjiSessions, WinjiSessions>();
                    services.AddScoped<IMaterialManagment, MaterialManagment>();
                    services.AddScoped<IAssessmentMaterial, AssessmentMaterial>();
                    services.AddScoped<IFileMaterial, FileMaterial>();
                    services.AddScoped<IYouTubeMaterial, YouTubeMaterial>();
                    services.AddScoped<IUrlMaterial,UrlMaterial>();
                    services.AddScoped<IVedioMaterial, VedioMaterial>();
                    services.AddScoped<IScormMaterial, ScormMaterial>();
                    services.AddScoped<IPoolMaterial, PoolMaterial>();
                    services.AddScoped<ISurvyMaterial, SurvyMaterial>();
                    services.AddScoped<IHtml5Material, Html5Material>();
                    services.AddScoped<IDiscussionMaterial, DiscussionMaterial>();
                });
        }
    }
}
