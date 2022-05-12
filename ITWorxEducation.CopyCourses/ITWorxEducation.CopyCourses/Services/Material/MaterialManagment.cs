using ITWorxEducation.CopyCourses.Enums;
using ITWorxEducation.CopyCourses.IServices.IMaterial;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.Services.Material
{
    public class MaterialManagment :IMaterialManagment 
    {
        private readonly IAssessmentMaterial _assessmentMaterial;
        private readonly IFileMaterial _fileMaterial;
        private readonly IYouTubeMaterial _youTubeMaterial;
        private readonly IUrlMaterial _urlMaterial;
        private readonly IVedioMaterial _vedioMaterial;
        private readonly IScormMaterial _scormMaterial;
        private readonly IPoolMaterial _poolMateril;
        private readonly ISurvyMaterial _survyMaterial;
        private readonly IHtml5Material _html5Material;
        private readonly IDiscussionMaterial _discussionMaterial;

        public MaterialManagment(IAssessmentMaterial assessmentMaterial,
            IFileMaterial fileMaterial,
            IYouTubeMaterial youTubeMaterial,
            IUrlMaterial urlMaterial,
            IVedioMaterial vedioMaterial,
            IScormMaterial scormMaterial,
            IPoolMaterial poolMateril,
            ISurvyMaterial survyMaterial,
            IHtml5Material html5Material,
            IDiscussionMaterial discussionMaterial
            )
        {
            _assessmentMaterial = assessmentMaterial;
            _fileMaterial = fileMaterial;
            _youTubeMaterial = youTubeMaterial;
            _urlMaterial = urlMaterial;
            _vedioMaterial = vedioMaterial;
            _scormMaterial = scormMaterial;
            _poolMateril = poolMateril;
            _survyMaterial = survyMaterial;
            _html5Material = html5Material;
            _discussionMaterial = discussionMaterial;
        }
        public string _materialTitle { get; set; }
        public string _contentParsing { get; set; }
        public JObject _materialJson { get; set; }
        public JObject _contentParsed { get; set; }
        public string sessionId { get; set; }
        public string roundId { get; set; }
        public string _tokenReceiver { get; set; }
        public void ManageAllMaterial(JObject sessionInfo, string sessionId, string roundId, string tokenReceiver,string tokenSender) 
        {
            JArray materialArray = JArray.Parse(sessionInfo["Materials"].ToString());
            for (int materialcount = 0; materialcount < materialArray.Count; materialcount++)
            {
                _materialTitle = materialArray[materialcount]["Title"].ToString();
                _contentParsing = sessionInfo["Materials"][materialcount]["Content"].ToString();
                string material = sessionInfo["Materials"][materialcount].ToString();
                _materialJson = JObject.Parse(material);
                _contentParsed = JObject.Parse(_contentParsing);
                string materialTypeId = materialArray[materialcount]["MaterialTypeId"].ToString();
                int materialEnum = int.Parse(materialTypeId);
                _tokenReceiver = tokenReceiver;
                switch(materialEnum)
                {
                    case (int)MaterialType.AssessmentsMaterial:
                    case (int)MaterialType.Quiz:
                    case (int)MaterialType.InCaseActivity:
                        string jsonContentAuth = _contentParsed["activityAuthorizationData"].ToString();
                        JObject Assessment = JObject.Parse(_contentParsed["assessment"].ToString());
                        _assessmentMaterial.SaveAssessments(Assessment, sessionId, int.Parse(roundId), materialEnum.ToString(), _materialTitle, _tokenReceiver, tokenSender, jsonContentAuth);
                        break;
                    case (int)MaterialType.FileMaterial:
                        _fileMaterial.SaveMaterialFile(_materialJson,_contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver, tokenSender);
                        break;
                    case (int)MaterialType.YoutubeMaterial:
                        _youTubeMaterial.SaveMaterialYoutube(_materialJson, _contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver);
                        break;
                    case (int)MaterialType.UrlMaterial:
                        _urlMaterial.SaveMaterialUrl(_materialJson, _contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver);
                        break;
                    case (int)MaterialType.VedioMaterial:
                        _vedioMaterial.SaveVedioMaterial(_materialJson, _contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver,tokenSender);
                        break;
                    case (int)MaterialType.ScormMaterial:
                        _scormMaterial.SaveScormMaterial(_materialJson, _contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver,tokenSender);
                        break;
                    case (int)MaterialType.PoolMaterial:
                        _poolMateril.SavePoolMaterial(_materialJson, _contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver);
                        break;
                    case (int)MaterialType.SurvyMaterial:
                        _survyMaterial.SaveSurvy(_materialJson, _contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver);
                        break;
                    case (int)MaterialType.Html5Material:
                        _html5Material.SaveHtml5(_materialJson, _contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver,tokenSender);
                        break;
                    case (int)MaterialType.DiscussionMaterial:
                        _discussionMaterial.SaveDiscussion(_materialJson, _contentParsed, sessionId, int.Parse(roundId), materialTypeId, _tokenReceiver);
                        break;
                }
            }
        }
    }
}
