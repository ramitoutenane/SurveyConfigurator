
using SurveyConfiguratorEntities;
using SurveyConfiguratorWeb.Properties;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SurveyConfiguratorWeb.Models
{
    public class QuestionModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext pControllerContext, ModelBindingContext pBindingContext, Type pModelType)
        {
            try
            {
                if (pModelType.Equals(typeof(BaseQuestion)))
                {
                    string tText = (string)pBindingContext.ValueProvider.GetValue(nameof(BaseQuestion.Text))?.ConvertTo(typeof(string));
                    tText = Regex.Replace(tText, @"\r\n?", "\n");
                    if (tText.Length > QuestionValidationValues.cQUESTION_TEXT_LENGTH)
                        pBindingContext.ModelState.AddModelError(nameof(BaseQuestion.Text), Errors.START_LARGER_THAN_END_ERROR);

                    Type tQuestionType = null;
                    int tTypeId = (int)pBindingContext.ValueProvider.GetValue(ConstantStringResources.cTYPE_ID)?.ConvertTo(typeof(int));
                    switch (tTypeId)
                    {
                        case (int)QuestionType.Slider:
                            tQuestionType = typeof(SliderQuestion);
                            int tStartValue;
                            int tEndValue;
                            if (int.TryParse(pBindingContext.ValueProvider.GetValue(nameof(SliderQuestion.StartValue)).AttemptedValue, out tStartValue) &&
                                int.TryParse(pBindingContext.ValueProvider.GetValue(nameof(SliderQuestion.EndValue)).AttemptedValue, out tEndValue))
                            {
                                if (tStartValue >= tEndValue)
                                    pBindingContext.ModelState.AddModelError(nameof(SliderQuestion.StartValue), Errors.START_LARGER_THAN_END_ERROR);
                            }
                            break;
                        case (int)QuestionType.Smiley:
                            tQuestionType = typeof(SmileyQuestion);
                            break;
                        case (int)QuestionType.Stars:
                            tQuestionType = typeof(StarsQuestion);
                            break;
                    }
                    var tQuestion = Activator.CreateInstance(tQuestionType);
                    pBindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, tQuestionType);
                    pBindingContext.ModelMetadata.Model = tQuestion;
                    return tQuestion;
                }
                return base.CreateModel(pControllerContext, pBindingContext, pModelType);
            }
            catch (Exception pError)
            {
                ErrorLogger.Log(pError);
                return base.CreateModel(pControllerContext, pBindingContext, pModelType);
            }
        }
    }
}