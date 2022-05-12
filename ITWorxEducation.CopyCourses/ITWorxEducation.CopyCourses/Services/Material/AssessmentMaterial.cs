using ITWorxEducation.CopyCourses.FileManagment;
using ITWorxEducation.CopyCourses.IServices.IMaterial;
using ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels;
using ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels.QuestionBodyViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.Services.Material
{
    public class AssessmentMaterial:IAssessmentMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SaveAssessments(JObject Assessment,string contextId, int parentContextId,string materialTypeId,string materialTitle,string _tokenReceiver, string _tokenSender, string jsonContentAuth)
        {
            JArray Questions = JArray.Parse(Assessment["Questions"]?.ToString());
            QuestionBase[] assessmentQuestions = new QuestionBase[Questions.Count];
            NewMaterial newMaterial = new NewMaterial
            {
                content = new NewMaterialContent { BadgeId = null },
                ContextId = contextId,
                ContextTypeId = 1,
                materialTypeId = int.Parse(materialTypeId),
                ParentContextId = parentContextId.ToString(),
                ParentContextTypeId = 2,
                Title = materialTitle
            };
            HttpClient httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenReceiver);
            string assessmentMaterialJson = JsonConvert.SerializeObject(newMaterial);
            var stringContentForAssessment = new StringContent(assessmentMaterialJson, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage addedMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForAssessment).Result;
            if (addedMaterial.IsSuccessStatusCode)
            {
                JObject AddedMaterialJson = JObject.Parse(addedMaterial.Content.ReadAsStringAsync().Result.ToString());
                JObject jsonContent = JObject.Parse(AddedMaterialJson["Content"].ToString());
                //Console.WriteLine(jsonContent["activityAuthorizationData"]);
                JArray JFileArray = JArray.Parse(Assessment["Files"]?.ToString());
                AssessmentFiles[] AssessmentFiles = new AssessmentFiles[JFileArray.Count];
                for (int fileCount = 0; fileCount < JFileArray.Count; fileCount++)
                {

                    string fileName = JFileArray[fileCount]["Name"]?.ToString();
                    string fileId = JFileArray[fileCount]["Id"]?.ToString();
                    string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
                    string result = FileOperations.FileManagment(fileId, fileName, extention, "-1", "2",_tokenReceiver, _tokenSender, jsonContentAuth);
                    JObject resultJson = JObject.Parse(result);
                    AssessmentFiles[fileCount] = new AssessmentFiles
                    {
                        Id = resultJson["Id"]?.ToString(),
                        ContentType = JFileArray[fileCount]["ContentType"]?.ToString(),
                        Size = int.Parse(JFileArray[fileCount]["Size"]?.ToString()),
                        extension = extention,
                        Name = fileName,
                        downloadUrl = $"{configuration["AuthorizationTokenSource:BaseURLAssessment"]}/api/Files/GetFile/" + fileId + "/" + fileName,
                        playUrl = $"{configuration["AuthorizationTokenSource:BaseURLAssessment"]}/api/Files/GetFile/" + fileId + "/" + fileName,
                        isAdded = true,
                        fileNameWithoutExtention = fileName.Split('.')[0]?.ToString(),
                    };
                }

                //Copy questions in The assessment

                for (int questionCount = 0; questionCount < Questions.Count; questionCount++)
                {
                    
                    string questionType = Questions[questionCount]["Type"]?.ToString();

                    if(questionType != "Comprehension")
                    {
                        InnerFile questionfile = null;
                        string downloadUrl = "";
                        string playUrl = "";
                        string fId = "";
                        string fName = "";
                        if (Questions[questionCount]["File"].ToString() != "")
                        {

                            JObject questionFileObject = JObject.Parse(Questions[questionCount]["File"]?.ToString());
                            string size = questionFileObject["Size"]?.ToString();
                            string ContentType = questionFileObject["ContentType"]?.ToString();
                            string id = questionFileObject["Id"]?.ToString();
                            string name = questionFileObject["Name"]?.ToString();
                            string fileExtention = name.Substring(name.LastIndexOf('.') + 1, (name.Length - (name.LastIndexOf('.') + 1)));
                            string questionFile = FileOperations.FileManagment(id, name, fileExtention, "-1", "2", _tokenReceiver, _tokenSender, jsonContentAuth);
                            JObject questionFileJson = JObject.Parse(questionFile);
                            downloadUrl = $"{configuration["AuthorizationTokenSource:BaseURLAssessment"]}/api/Files/GetFile/" + questionFileJson["Id"]?.ToString() + "/" + questionFileJson["Name"]?.ToString();
                            fId = questionFileJson["Id"]?.ToString();
                            fName = questionFileJson["Name"]?.ToString();
                            playUrl = $"{configuration["AuthorizationTokenSource:BaseURLAssessment"]}/api/Files/GetFile/" + questionFileJson["Id"]?.ToString() + "/" + questionFileJson["Name"]?.ToString();
                            questionfile = new InnerFile
                            {
                                Id = questionFileJson["Id"]?.ToString(),
                                ContentType = ContentType,
                                Name = questionFileJson["Name"]?.ToString(),
                                Size = int.Parse(size)
                            };
                        }

                        //Console.WriteLine(Questions[questionCount]["Type"]?.ToString());

                        Console.WriteLine(questionType);
                        JArray JOptionArray = new JArray();

                        if (questionType != "Formula" && questionType != "FillInTheBlanks" && questionType != "Comprehension" && questionType != "Essay" && questionType != "ShortAnswer")
                        {
                            JOptionArray = JArray.Parse(Questions[questionCount]["Body"]["Options"].ToString());
                        }

                        object body = null;
                        switch (questionType)
                        {
                            case "Essay":
                            case "ShortAnswer":
                                body = new { Options = new object[] { } };
                                break;
                            case "MCQ":
                                body = QuestionBodyManagment.QuestionBodyBasic(JOptionArray);
                                break;
                            case "TrueFalse":
                                body = QuestionBodyManagment.QuestionBodyBasic(JOptionArray);
                                break;
                            case "MultiResponse":
                                body = QuestionBodyManagment.QuestionBodyBasic(JOptionArray);
                                break;

                            case "Matching":
                                JArray JFixedOptions = JArray.Parse(Questions[questionCount]["Body"]["FixedOptions"].ToString());
                                body = QuestionBodyManagment.QuestionBodyMatching(JOptionArray, JFixedOptions);
                                break;
                            case "Order":
                                body = QuestionBodyManagment.QuestionBodyOrder(JOptionArray);
                                break;
                            case "Image":

                                body = new Body
                                {
                                    Options = new Options[] { }
                                };
                                break;
                            case "Formula":
                                JObject jBody = JObject.Parse(Questions[questionCount]["Body"].ToString());
                                JArray jVariables = JArray.Parse(jBody["Variables"].ToString());
                                body = QuestionBodyManagment.QuestionBodyFormula(jBody, jVariables);
                                break;
                            case "FillInTheBlanks":
                                JObject jBodyBlank = JObject.Parse(Questions[questionCount]["Body"].ToString());
                                body = QuestionBodyManagment.QuestionBodyFillInTheBlanks(jBodyBlank);
                                break;
                        }

                        if (Questions[questionCount]["File"].ToString() != "")
                        {
                            assessmentQuestions[questionCount] = new QuestionWithFile
                            {
                                Title = Questions[questionCount]["Title"]?.ToString(),
                                QuestionTypeUniqueName = Questions[questionCount]["QuestionTypeUniqueName"]?.ToString(),
                                id = 0,
                                DifficultyLevel = Questions[questionCount]["DifficultyLevel"]?.ToString(),
                                DifficultyLevelId = Questions[questionCount]["DifficultyLevelId"]?.ToString(),
                                Grade = int.Parse(Questions[questionCount]["Grade"]?.ToString()),
                                Hint = Questions[questionCount]["Hint"]?.ToString(),
                                IsOccurrenceMessage = Questions[questionCount]["IsOccurrenceMessage"]?.ToString(),
                                ObjectState = Questions[questionCount]["ObjectState"]?.ToString(),
                                Order = int.Parse(Questions[questionCount]["Order"]?.ToString()),
                                Type = Questions[questionCount]["Type"]?.ToString(),
                                clientId = Questions[questionCount]["clientId"]?.ToString(),
                                PreviewMode = Questions[questionCount]["PreviewMode"],
                                indexforcorrect = Questions[questionCount]["indexforcorrect"],
                                CreateMode = Questions[questionCount]["CreateMode"],
                                File = questionfile,
                                DownloadUrl = downloadUrl,
                                PlayUrl = playUrl,
                                FileId = fId,
                                FileName = fName,
                                Body = body,
                                Thumbnail = Questions[questionCount]["Thumbnail"]?.ToString(),
                                //EnableShuffleAnswers = bool.Parse(Questions[questionCount]["EnableShuffleAnswers"]?.ToString())
                            };
                        }
                        else
                        {
                            if (questionType != "Image")
                            {
                                assessmentQuestions[questionCount] = new Question
                                {
                                    Title = Questions[questionCount]["Title"]?.ToString(),
                                    QuestionTypeUniqueName = Questions[questionCount]["QuestionTypeUniqueName"]?.ToString(),
                                    id = 0,
                                    DifficultyLevel = Questions[questionCount]["DifficultyLevel"]?.ToString(),
                                    DifficultyLevelId = Questions[questionCount]["DifficultyLevelId"]?.ToString(),
                                    Grade = int.Parse(Questions[questionCount]["Grade"]?.ToString()),
                                    Hint = Questions[questionCount]["Hint"]?.ToString(),
                                    IsOccurrenceMessage = Questions[questionCount]["IsOccurrenceMessage"]?.ToString(),
                                    ObjectState = Questions[questionCount]["ObjectState"]?.ToString(),
                                    Order = int.Parse(Questions[questionCount]["Order"]?.ToString()),
                                    Type = Questions[questionCount]["Type"]?.ToString(),
                                    clientId = Questions[questionCount]["clientId"]?.ToString(),
                                    PreviewMode = Questions[questionCount]["PreviewMode"],
                                    indexforcorrect = Questions[questionCount]["indexforcorrect"],
                                    CreateMode = Questions[questionCount]["CreateMode"],
                                    Body = body,
                                    Thumbnail = Questions[questionCount]["Thumbnail"]?.ToString(),
                                    //EnableShuffleAnswers = bool.Parse(Questions[questionCount]["EnableShuffleAnswers"]?.ToString())
                                };
                            }
                            else
                            {
                                JObject jQuestionImage = JObject.Parse(Questions[questionCount].ToString());
                                string id = jQuestionImage["QuestionImageFileId"]?.ToString();
                                string name = jQuestionImage["QuestionImageFile"]["Name"]?.ToString();
                                string fileExtention = name.Substring(name.LastIndexOf('.') + 1, (name.Length - (name.LastIndexOf('.') + 1)));
                                string questionImage = FileOperations.FileManagment(id, name, fileExtention, "-1", "2", _tokenReceiver, _tokenSender, jsonContentAuth);
                                JObject jquestionImage = JObject.Parse(questionImage);
                                //Console.WriteLine(questionImage);
                                string imageContent = jQuestionImage["QuestionImageFile"]["ContentType"]?.ToString();

                                assessmentQuestions[questionCount] = new QuestionWithImage
                                {
                                    Title = Questions[questionCount]["Title"]?.ToString(),
                                    QuestionTypeUniqueName = Questions[questionCount]["QuestionTypeUniqueName"]?.ToString(),
                                    id = 0,
                                    DifficultyLevel = Questions[questionCount]["DifficultyLevel"]?.ToString(),
                                    DifficultyLevelId = Questions[questionCount]["DifficultyLevelId"]?.ToString(),
                                    Grade = int.Parse(Questions[questionCount]["Grade"]?.ToString()),
                                    Hint = Questions[questionCount]["Hint"]?.ToString(),
                                    IsOccurrenceMessage = Questions[questionCount]["IsOccurrenceMessage"]?.ToString(),
                                    ObjectState = Questions[questionCount]["ObjectState"]?.ToString(),
                                    Order = int.Parse(Questions[questionCount]["Order"]?.ToString()),
                                    Type = Questions[questionCount]["Type"]?.ToString(),
                                    clientId = Questions[questionCount]["clientId"]?.ToString(),
                                    PreviewMode = Questions[questionCount]["PreviewMode"],
                                    indexforcorrect = Questions[questionCount]["indexforcorrect"],
                                    CreateMode = Questions[questionCount]["CreateMode"],
                                    Body = body,
                                    Thumbnail = Questions[questionCount]["Thumbnail"]?.ToString(),
                                    QuestionImageFile = new QuestionImageFile
                                    {
                                        ContentType = imageContent,
                                        Id = jquestionImage["Id"]?.ToString(),
                                        Name = jquestionImage["Name"]?.ToString(),
                                        Size = int.Parse(jquestionImage["Size"]?.ToString())
                                    }
                                };
                            }
                        }
                    }
                    else
                    {
                        JArray subquestions = JArray.Parse(Questions[questionCount]["SubQuestions"].ToString());
                        QuestionBase[] subQuest = new QuestionBase[subquestions.Count];
                        for (int sQuestion = 0; sQuestion < subquestions.Count; sQuestion++)
                        {
                            string questionSubType = subquestions[sQuestion]["Type"]?.ToString();
                            InnerFile questionfile = null;
                            string downloadUrl = "";
                            string playUrl = "";
                            string fId = "";
                            string fName = "";
                            if (subquestions[sQuestion]["File"].ToString() != "")
                            {

                                JObject questionFileObject = JObject.Parse(subquestions[sQuestion]["File"]?.ToString());
                                string size = questionFileObject["Size"]?.ToString();
                                string ContentType = questionFileObject["ContentType"]?.ToString();
                                string id = questionFileObject["Id"]?.ToString();
                                string name = questionFileObject["Name"]?.ToString();
                                string fileExtention = name.Substring(name.LastIndexOf('.') + 1, (name.Length - (name.LastIndexOf('.') + 1)));
                                string questionFile = FileOperations.FileManagment(id, name, fileExtention, "-1", "2", _tokenReceiver, _tokenSender, jsonContentAuth);
                                JObject questionFileJson = JObject.Parse(questionFile);
                                downloadUrl = $"{configuration["AuthorizationTokenSource:BaseURLAssessment"]}/api/Files/GetFile/" + questionFileJson["Id"]?.ToString() + "/" + questionFileJson["Name"]?.ToString();
                                fId = questionFileJson["Id"]?.ToString();
                                fName = questionFileJson["Name"]?.ToString();
                                playUrl = $"{configuration["AuthorizationTokenSource:BaseURLAssessment"]}/api/Files/GetFile/" + questionFileJson["Id"]?.ToString() + "/" + questionFileJson["Name"]?.ToString();
                                questionfile = new InnerFile
                                {
                                    Id = questionFileJson["Id"]?.ToString(),
                                    ContentType = ContentType,
                                    Name = questionFileJson["Name"]?.ToString(),
                                    Size = int.Parse(size)
                                };
                            }

                            //Console.WriteLine(Questions[questionCount]["Type"]?.ToString());

                            Console.WriteLine(questionType);
                            JArray JOptionArray = new JArray();

                            if (questionSubType != "Formula" && questionSubType != "FillInTheBlanks" && questionSubType != "Comprehension" && questionSubType != "Essay" && questionSubType != "ShortAnswer")
                            {
                                JOptionArray = JArray.Parse(subquestions[sQuestion]["Body"]["Options"].ToString());
                            }

                            object body = null;
                            switch (questionSubType)
                            {
                                case "Essay":
                                case "ShortAnswer":
                                    body = new { Options = new object[] { } };
                                    break;
                                case "MCQ":
                                    body = QuestionBodyManagment.QuestionBodyBasic(JOptionArray);
                                    break;
                                case "TrueFalse":
                                    body = QuestionBodyManagment.QuestionBodyBasic(JOptionArray);
                                    break;
                                case "MultiResponse":
                                    body = QuestionBodyManagment.QuestionBodyBasic(JOptionArray);
                                    break;

                                case "Matching":
                                    JArray JFixedOptions = JArray.Parse(subquestions[sQuestion]["Body"]["FixedOptions"].ToString());
                                    body = QuestionBodyManagment.QuestionBodyMatching(JOptionArray, JFixedOptions);
                                    break;
                                case "Order":
                                    body = QuestionBodyManagment.QuestionBodyOrder(JOptionArray);
                                    break;
                                case "Image":

                                    body = new Body
                                    {
                                        Options = new Options[] { }
                                    };
                                    break;
                                case "Formula":
                                    JObject jBody = JObject.Parse(subquestions[sQuestion]["Body"].ToString());
                                    JArray jVariables = JArray.Parse(jBody["Variables"].ToString());
                                    body = QuestionBodyManagment.QuestionBodyFormula(jBody, jVariables);
                                    break;
                                case "FillInTheBlanks":
                                    JObject jBodyBlank = JObject.Parse(subquestions[sQuestion]["Body"].ToString());
                                    body = QuestionBodyManagment.QuestionBodyFillInTheBlanks(jBodyBlank);
                                    break;
                            }

                            if (subquestions[sQuestion]["File"].ToString() != "")
                            {
                                subQuest[sQuestion] = new QuestionWithFile
                                {
                                    Title = subquestions[sQuestion]["Title"]?.ToString(),
                                    QuestionTypeUniqueName = subquestions[sQuestion]["QuestionTypeUniqueName"]?.ToString(),
                                    id = 0,
                                    DifficultyLevel = subquestions[sQuestion]["DifficultyLevel"]?.ToString(),
                                    DifficultyLevelId = subquestions[sQuestion]["DifficultyLevelId"]?.ToString(),
                                    Grade = int.Parse(subquestions[sQuestion]["Grade"]?.ToString()),
                                    Hint = subquestions[sQuestion]["Hint"]?.ToString(),
                                    IsOccurrenceMessage = subquestions[sQuestion]["IsOccurrenceMessage"]?.ToString(),
                                    ObjectState = subquestions[sQuestion]["ObjectState"]?.ToString(),
                                    Order = int.Parse(subquestions[sQuestion]["Order"]?.ToString()),
                                    Type = subquestions[sQuestion]["Type"]?.ToString(),
                                    clientId = subquestions[sQuestion]["clientId"]?.ToString(),
                                    PreviewMode = subquestions[sQuestion]["PreviewMode"],
                                    indexforcorrect = subquestions[sQuestion]["indexforcorrect"],
                                    CreateMode = subquestions[sQuestion]["CreateMode"],
                                    File = questionfile,
                                    DownloadUrl = downloadUrl,
                                    PlayUrl = playUrl,
                                    FileId = fId,
                                    FileName = fName,
                                    Body = body,
                                    Thumbnail = subquestions[sQuestion]["Thumbnail"]?.ToString(),
                                    //EnableShuffleAnswers = bool.Parse(Questions[questionCount]["EnableShuffleAnswers"]?.ToString())
                                };
                            }
                            else
                            {
                                if (questionSubType != "Image")
                                {
                                    subQuest[sQuestion] = new Question
                                    {
                                        Title = subquestions[sQuestion]["Title"]?.ToString(),
                                        QuestionTypeUniqueName = subquestions[sQuestion]["QuestionTypeUniqueName"]?.ToString(),
                                        id = 0,
                                        DifficultyLevel = subquestions[sQuestion]["DifficultyLevel"]?.ToString(),
                                        DifficultyLevelId = subquestions[sQuestion]["DifficultyLevelId"]?.ToString(),
                                        Grade = int.Parse(subquestions[sQuestion]["Grade"]?.ToString()),
                                        Hint = subquestions[sQuestion]["Hint"]?.ToString(),
                                        IsOccurrenceMessage = subquestions[sQuestion]["IsOccurrenceMessage"]?.ToString(),
                                        ObjectState = subquestions[sQuestion]["ObjectState"]?.ToString(),
                                        Order = int.Parse(subquestions[sQuestion]["Order"]?.ToString()),
                                        Type = subquestions[sQuestion]["Type"]?.ToString(),
                                        clientId = subquestions[sQuestion]["clientId"]?.ToString(),
                                        PreviewMode = subquestions[sQuestion]["PreviewMode"],
                                        indexforcorrect = subquestions[sQuestion]["indexforcorrect"],
                                        CreateMode = subquestions[sQuestion]["CreateMode"],
                                        Body = body,
                                        Thumbnail = subquestions[sQuestion]["Thumbnail"]?.ToString(),
                                        //EnableShuffleAnswers = bool.Parse(Questions[questionCount]["EnableShuffleAnswers"]?.ToString())
                                    };
                                }
                                else
                                {
                                    JObject jQuestionImage = JObject.Parse(subquestions[sQuestion].ToString());
                                    string id = jQuestionImage["QuestionImageFileId"]?.ToString();
                                    string name = jQuestionImage["QuestionImageFile"]["Name"]?.ToString();
                                    string fileExtention = name.Substring(name.LastIndexOf('.') + 1, (name.Length - (name.LastIndexOf('.') + 1)));
                                    string questionImage = FileOperations.FileManagment(id, name, fileExtention, "-1", "2", _tokenReceiver, _tokenSender, jsonContentAuth);
                                    JObject jquestionImage = JObject.Parse(questionImage);
                                    //Console.WriteLine(questionImage);
                                    string imageContent = jQuestionImage["QuestionImageFile"]["ContentType"]?.ToString();

                                    subQuest[sQuestion] = new QuestionWithImage
                                    {
                                        Title = subquestions[sQuestion]["Title"]?.ToString(),
                                        QuestionTypeUniqueName = subquestions[sQuestion]["QuestionTypeUniqueName"]?.ToString(),
                                        id = 0,
                                        DifficultyLevel = subquestions[sQuestion]["DifficultyLevel"]?.ToString(),
                                        DifficultyLevelId = subquestions[sQuestion]["DifficultyLevelId"]?.ToString(),
                                        Grade = int.Parse(subquestions[sQuestion]["Grade"]?.ToString()),
                                        Hint = subquestions[sQuestion]["Hint"]?.ToString(),
                                        IsOccurrenceMessage = subquestions[sQuestion]["IsOccurrenceMessage"]?.ToString(),
                                        ObjectState = subquestions[sQuestion]["ObjectState"]?.ToString(),
                                        Order = int.Parse(subquestions[sQuestion]["Order"]?.ToString()),
                                        Type = subquestions[sQuestion]["Type"]?.ToString(),
                                        clientId = subquestions[sQuestion]["clientId"]?.ToString(),
                                        PreviewMode = subquestions[sQuestion]["PreviewMode"],
                                        indexforcorrect = subquestions[sQuestion]["indexforcorrect"],
                                        CreateMode = subquestions[sQuestion]["CreateMode"],
                                        Body = body,
                                        Thumbnail = subquestions[sQuestion]["Thumbnail"]?.ToString(),
                                        QuestionImageFile = new QuestionImageFile
                                        {
                                            ContentType = imageContent,
                                            Id = jquestionImage["Id"]?.ToString(),
                                            Name = jquestionImage["Name"]?.ToString(),
                                            Size = int.Parse(jquestionImage["Size"]?.ToString())
                                        }
                                    };
                                }
                            }
                        }

                        assessmentQuestions[questionCount] = new WrapperQuestions 
                        {
                            SubQuestions = subQuest,
                            clientId = Questions[questionCount]["clientId"]?.ToString(),
                            CreateMode = Questions[questionCount]["CreateMode"],
                            Grade = int.Parse(Questions[questionCount]["Grade"]?.ToString()),
                            id = 0,
                            Title = Questions[questionCount]["Title"]?.ToString(),
                            indexforcorrect = Questions[questionCount]["indexforcorrect"],
                            IsOccurrenceMessage = Questions[questionCount]["IsOccurrenceMessage"]?.ToString(),
                            ObjectState = Questions[questionCount]["ObjectState"]?.ToString(),
                            Order = int.Parse(Questions[questionCount]["Order"]?.ToString()),
                            PreviewMode = Questions[questionCount]["PreviewMode"],
                            QuestionTypeUniqueName = Questions[questionCount]["QuestionTypeUniqueName"]?.ToString(),
                            Type = Questions[questionCount]["Type"]?.ToString(),
                            Body = new
                            {
                                ParagraphPhrase = Questions[questionCount]["Body"]["ParagraphPhrase"]
                            }
                        };
                        

                    }
                }

                   

                Assessment assessment = null;
                if (Assessment["Files"]?.ToString() != "")
                {
                    Console.WriteLine("  1  "+Assessment["AllowExceedingTime"]?.ToString() +"  2  "+ Assessment["AllowLateSubmission"]?.ToString()+"  3  "+ Assessment["EnablePublishingScore"]?.ToString()+"  4  "+ Assessment["IsActive"]?.ToString()+ "  5  "+Assessment["IsAssessmentProctored"]?.ToString()+ "  6  " +Assessment["IsGroupAssignees"]?.ToString()+ "  7  " +Assessment["IsProject"]?.ToString()+"  8  "+ Assessment["IsSpecificAssignees"]?.ToString());
                    assessment = new AssessmentWithFile
                    {
                        AllowExceedingTime = bool.Parse(Assessment["AllowExceedingTime"]?.ToString()),
                        AllowLateSubmission = bool.Parse(Assessment["AllowLateSubmission"]?.ToString()),
                        AssessmentExternalTool = Assessment["AssessmentExternalTool"],
                        CreatedBy = Assessment["CreatedBy"]?.ToString(),
                        Description = Assessment["Description"]?.ToString(),
                        DueDate = Assessment["DueDate"],
                        Duration = Assessment["Duration"],
                        EnablePublishingScore = (bool?)Assessment["EnablePublishingScore"],
                        GradableItemId = Assessment["GradableItemId"]?.ToString(),
                        Grade = int.Parse(Assessment["Grade"]?.ToString()),
                        GradingMode = Assessment["GradingMode"]?.ToString(),
                        GradingPeriodId = Assessment["GradingPeriodId"]?.ToString(),
                        IsActive = bool.Parse(Assessment["IsActive"]?.ToString()),
                        Mode = int.Parse(Assessment["ModeId"]?.ToString()),
                        IsAssessmentProctored = (bool?)Assessment["IsAssessmentProctored"],
                        IsGroupAssignees = bool.Parse(Assessment["IsGroupAssignees"]?.ToString()),
                        IsSpecificAssignees = bool.Parse(Assessment["IsSpecificAssignees"]?.ToString()),
                        Title = Assessment["Title"]?.ToString(),
                        Type = Assessment["Type"]?.ToString(),
                        Status = Assessment["Status"]?.ToString(),
                        PassingScore = int.Parse(Assessment["PassingScore"]?.ToString()),
                        ProctoredConfiguration = Assessment["ProctoredConfiguration"],
                        Time = Assessment["Time"],
                        RubricId = Assessment["RubricId"],
                        ScormFile = Assessment["ScormFile"],
                        RandomizationCriteriaData = Assessment["RandomizationCriteriaData"],
                        SubmissionType = Assessment["SubmissionType"]?.ToString(),
                        MetaData = new MetaData
                        {
                            CourseId = parentContextId.ToString(),
                            MaterialId = AddedMaterialJson["Id"]?.ToString(),
                            OrganizationId = Assessment["MetaData"]["OrganizationId"]?.ToString(),
                            SchoolId = Assessment["MetaData"]["SchoolId"],
                            SessionId = contextId
                        },
                        Files = AssessmentFiles,
                        Questions = assessmentQuestions
                    };
                }

                else
                {
                    assessment = new Assessment
                    {
                        AllowExceedingTime = bool.Parse(Assessment["AllowExceedingTime"]?.ToString()),
                        AllowLateSubmission = bool.Parse(Assessment["AllowLateSubmission"]?.ToString()),
                        AssessmentExternalTool = Assessment["AssessmentExternalTool"],
                        CreatedBy = Assessment["CreatedBy"]?.ToString(),
                        Description = Assessment["Description"]?.ToString(),
                        DueDate = Assessment["DueDate"],
                        Duration = Assessment["Duration"],
                        EnablePublishingScore = (bool?)Assessment["EnablePublishingScore"],
                        GradableItemId = Assessment["GradableItemId"]?.ToString(),
                        Grade = int.Parse(Assessment["Grade"]?.ToString()),
                        GradingMode = Assessment["GradingMode"]?.ToString(),
                        GradingPeriodId = Assessment["GradingPeriodId"]?.ToString(),
                        IsActive = bool.Parse(Assessment["IsActive"]?.ToString()),
                        Mode = int.Parse(Assessment["ModeId"]?.ToString()),
                        IsAssessmentProctored = (bool?)Assessment["IsAssessmentProctored"],
                        IsGroupAssignees = bool.Parse(Assessment["IsGroupAssignees"]?.ToString()),
                        IsSpecificAssignees = bool.Parse(Assessment["IsSpecificAssignees"]?.ToString()),
                        Title = Assessment["Title"]?.ToString(),
                        Type = Assessment["Type"]?.ToString(),
                        Status = Assessment["Status"]?.ToString(),
                        PassingScore = int.Parse(Assessment["PassingScore"]?.ToString()),
                        ProctoredConfiguration = Assessment["ProctoredConfiguration"],
                        Time = Assessment["Time"],
                        RubricId = Assessment["RubricId"],
                        ScormFile = Assessment["ScormFile"],
                        RandomizationCriteriaData = Assessment["RandomizationCriteriaData"],
                        SubmissionType = Assessment["SubmissionType"]?.ToString(),
                        MetaData = new MetaData
                        {
                            CourseId = parentContextId.ToString(),
                            MaterialId = AddedMaterialJson["Id"]?.ToString(),
                            OrganizationId = Assessment["MetaData"]["OrganizationId"]?.ToString(),
                            SchoolId = Assessment["MetaData"]["SchoolId"],
                            SessionId = contextId
                        },
                        Questions = assessmentQuestions
                    };
                }
                object[] bulkAssessments = new object[]
                {
                    new
                    {
                        Assessment = assessment,
                        AssessmentRequestID = "8dfafaf1-2e5f-4bc7-9a72-b63888c3d608",
                        EncryptedAuthorizationData = jsonContent["activityAuthorizationData"].ToString(),
                        ObjectState = "added",
                        Permissions = new object[]{ },
                        Questions = new object[]{ }
                    }
                };

                string JsonSerializeForAddingAssesment = JsonConvert.SerializeObject(bulkAssessments);
                JArray TestLog = JArray.Parse(JsonSerializeForAddingAssesment);
                Console.WriteLine(TestLog);
                var stringContentForAddingAssessment = new StringContent(JsonSerializeForAddingAssesment, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessageForAddingAssessment = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURLAssessment"]}/api/Assessments", stringContentForAddingAssessment).Result;
                if (httpResponseMessageForAddingAssessment.IsSuccessStatusCode)
                {
                    JArray jsonParsingForAddingAssessment = JArray.Parse(httpResponseMessageForAddingAssessment.Content.ReadAsStringAsync().Result);
                    Console.WriteLine(jsonParsingForAddingAssessment);
                }
            }
        }
    }
}
