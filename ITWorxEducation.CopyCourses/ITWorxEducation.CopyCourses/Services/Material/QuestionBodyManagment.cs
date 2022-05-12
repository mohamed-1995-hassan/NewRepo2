using ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels.QuestionBodyViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.Services.Material
{
    public class QuestionBodyManagment
    {
        public static Object QuestionBodyBasic(JArray JOptionArray)
        {
            Options[] options = new Options[JOptionArray.Count];
            for (int optionCount = 0; optionCount < JOptionArray.Count; optionCount++)
            {
                options[optionCount] = new Options
                {
                    Id = int.Parse(JOptionArray[optionCount]["Id"].ToString()),
                    Title = JOptionArray[optionCount]["Title"].ToString(),
                    IsCorrect = bool.Parse(JOptionArray[optionCount]["IsCorrect"].ToString())
                };
            }

            Body body = new Body
            {
                Options = options
            };
            return body;
        }

        public static Object QuestionBodyMatching(JArray JOptionArray, JArray JFixedOptions)
        {
            MatchingOrderOptions[] MatchingOptions = new MatchingOrderOptions[JOptionArray.Count];
            FixedOptions[] FixedOptions = new FixedOptions[JOptionArray.Count];
            for (int optionCount = 0; optionCount < JOptionArray.Count; optionCount++)
            {
                MatchingOptions[optionCount] = new MatchingOrderOptions
                {
                    Id = int.Parse(JOptionArray[optionCount]["Id"].ToString()),
                    Title = JOptionArray[optionCount]["Title"].ToString(),
                    IsCorrect = bool.Parse(JOptionArray[optionCount]["IsCorrect"].ToString()),
                    Order = int.Parse(JOptionArray[optionCount]["Order"].ToString())
                };
                FixedOptions[optionCount] = new FixedOptions
                {
                    Title = JFixedOptions[optionCount]["Title"]?.ToString()
                };
            }

            MatchingBody body = new MatchingBody
            {
                Options = MatchingOptions,
                FixedOptions = FixedOptions
            };
            return body;
        }

        public static Object QuestionBodyOrder(JArray JOptionArray)
        {
            MatchingOrderOptions[] OrderOptions = new MatchingOrderOptions[JOptionArray.Count];
            for (int optionCount = 0; optionCount < JOptionArray.Count; optionCount++)
            {
                OrderOptions[optionCount] = new MatchingOrderOptions
                {
                    Id = int.Parse(JOptionArray[optionCount]["Id"].ToString()),
                    Title = JOptionArray[optionCount]["Title"].ToString(),
                    IsCorrect = bool.Parse(JOptionArray[optionCount]["IsCorrect"].ToString()),
                    Order = int.Parse(JOptionArray[optionCount]["Order"].ToString())
                };
            }

            BodyOrder body = new BodyOrder
            {
                Options = OrderOptions
            };
            return body;
        }
        public static Object QuestionBodyFormula(JObject jBody, JArray jVariables)
        {
            Variables[] variables = new Variables[jVariables.Count];
            for (int variableNumber = 0; variableNumber < jVariables.Count; variableNumber++)
            {
                variables[variableNumber] = new Variables
                {
                    Dataset = jVariables[variableNumber]["Dataset"]?.ToString(),
                    exampleValue = int.Parse(jVariables[variableNumber]["exampleValue"]?.ToString()),
                    Max = int.Parse(jVariables[variableNumber]["Max"]?.ToString()),
                    Min = int.Parse(jVariables[variableNumber]["Min"]?.ToString()),
                    Name = jVariables[variableNumber]["Name"]?.ToString(),
                    VariableDataType = int.Parse(jVariables[variableNumber]["VariableDataType"]?.ToString())
                };
            }

            FormulaBody body = new FormulaBody
            {
                Variables = variables,
                ErrorMargin = jBody["ErrorMargin"]?.ToString(),
                Formula = jBody["Formula"]?.ToString(),
                FormulaDecimalPoint = jBody["FormulaDecimalPoint"]?.ToString()
            };
            return body;
        }

        public static Object QuestionBodyFillInTheBlanks(JObject jBodyBlank)
        {
            JArray jBlankAnswer = JArray.Parse(jBodyBlank["BlankAnswers"].ToString());
            BlankAnswers[] blankAnswers = new BlankAnswers[jBlankAnswer.Count];
            for (int blankAnswerNumber = 0; blankAnswerNumber < jBlankAnswer.Count; blankAnswerNumber++)
            {
                JArray jOption = JArray.Parse(jBlankAnswer[blankAnswerNumber]["Options"].ToString());
                MatchingOrderOptions[] options = new MatchingOrderOptions[jOption.Count];
                for (int optionNumber = 0; optionNumber < jOption.Count; optionNumber++)
                {
                    options[optionNumber] = new MatchingOrderOptions
                    {
                        Id = int.Parse(jOption[optionNumber]["Id"]?.ToString()),
                        IsCorrect = bool.Parse(jOption[optionNumber]["IsCorrect"]?.ToString()),
                        Order = int.Parse(jOption[optionNumber]["Order"]?.ToString()),
                        Title = jOption[optionNumber]["Title"]?.ToString()
                    };
                }

                blankAnswers[blankAnswerNumber] = new BlankAnswers
                {
                    Options = options,
                    Order = int.Parse(jBlankAnswer[blankAnswerNumber]["Order"]?.ToString()),
                    tempOption = jBlankAnswer[blankAnswerNumber]["Order"]?.ToString()
                };
            }
            BlankBody body = new BlankBody
            {
                BlankAnswers = blankAnswers
            };
            return body;
        }
    }
}
