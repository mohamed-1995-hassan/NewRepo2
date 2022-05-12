using ITWorxEducation.CopyCourses.Helpers;
using ITWorxEducation.CopyCourses.IServices;
using ITWorxEducation.CopyCourses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.Services
{
    public class OrganizationManegment : IOrganizationManegment
    {
        private readonly ICallBackRestApi _callBackRestApi;

        public OrganizationManegment(ICallBackRestApi callBackRestApi)
        {
            _callBackRestApi = callBackRestApi;
        }
        public string LoginToOrganization(string link,OrganizationLoginViewModel organizationLoginViewModel)
        {
            FormUrlEncodedContent formUrlEncodedContent = AddCredintilToFormData(organizationLoginViewModel);
            string login = _callBackRestApi.CallAPi(link, HttpMethod.Post,"",formUrlEncodedContent);
            return login;
        }
        public FormUrlEncodedContent AddCredintilToFormData(OrganizationLoginViewModel organizationLoginViewModel)
        {
            Dictionary<string, string> formContentValues = new Dictionary<string, string>();
            formContentValues.Add("grant_type", organizationLoginViewModel.grant_type);
            formContentValues.Add("username", organizationLoginViewModel.username);
            formContentValues.Add("password", organizationLoginViewModel.password);
            formContentValues.Add("client_id", organizationLoginViewModel.client_id);
            formContentValues.Add("client_secret", organizationLoginViewModel.client_secret);
            FormUrlEncodedContent formContent = new FormUrlEncodedContent(formContentValues);
            return formContent;
        }
    }
}
