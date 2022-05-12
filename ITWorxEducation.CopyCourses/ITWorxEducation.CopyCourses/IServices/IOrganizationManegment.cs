using ITWorxEducation.CopyCourses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices
{
    public interface IOrganizationManegment
    {
        public string LoginToOrganization(string link,OrganizationLoginViewModel organizationLoginViewModel);
        public FormUrlEncodedContent AddCredintilToFormData(OrganizationLoginViewModel organizationLoginViewModel);
    }
}
