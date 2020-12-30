
using SurveyConfiguratorEntities;
using System;
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
                    Type tQuestionType = null;
                    int tTypeId = (int)pBindingContext.ValueProvider.GetValue(ConstantStringResources.cTYPE_ID).ConvertTo(typeof(int));
                    switch (tTypeId)
                    {
                        case (int)QuestionType.Slider:
                            tQuestionType = typeof(SliderQuestion);
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