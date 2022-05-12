using ITWorxEducation.CopyCourses.Helpers.JsonConverterManagment;
using ITWorxEducation.CopyCourses.IServices;
using ITWorxEducation.CopyCourses.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace ITWorxEducation.CopyCourses.ApplicationBootStrap
{
    public class ToolStart
    {
        IConfiguration configuration = Program.LoadConfiguration();
        private readonly IWinjiCourses _winjiCourses;
        private readonly IOrganizationManegment _organizationManegment;

        public ToolStart(IWinjiCourses winjiCourses, IOrganizationManegment organizationManegment)
        {
            _winjiCourses = winjiCourses;
            _organizationManegment = organizationManegment;
        }
        public void Run()
        {
            try
            {
               string loginSender = _organizationManegment.LoginToOrganization(configuration["AuthorizationTokenSource:TokenUrl"], new OrganizationLoginViewModel
               {
                    client_id = configuration["AuthorizationTokenSource:client_id"],
                    client_secret = configuration["AuthorizationTokenSource:client_secret"],
                    grant_type = configuration["AuthorizationTokenSource:grant_type"],
                    password = configuration["AuthorizationTokenSource:Password"],
                    username = configuration["AuthorizationTokenSource:Username"]
               });

                string loginReceiver = _organizationManegment.LoginToOrganization(configuration["AuthorizationTokenDestination:TokenUrl"], new OrganizationLoginViewModel
                {
                    client_id = configuration["AuthorizationTokenDestination:client_id"],
                    client_secret = configuration["AuthorizationTokenDestination:client_secret"],
                    grant_type = configuration["AuthorizationTokenDestination:grant_type"],
                    password = configuration["AuthorizationTokenDestination:Password"],
                    username = configuration["AuthorizationTokenDestination:Username"]
                });
                LoginResponseViewModel loginSenderResult = JsonManagment.DeserializeJson<LoginResponseViewModel>(loginSender);
                LoginResponseViewModel loginReceiverResult = JsonManagment.DeserializeJson<LoginResponseViewModel>(loginReceiver);
                _winjiCourses.GetWinjiCourses(loginSenderResult.access_token, loginReceiverResult.access_token);
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
