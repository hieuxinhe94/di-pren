using System.Collections.Generic;
using Di.Common.Utils;
using EPiServer.Validation;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Partials.OrderFlow;

namespace Pren.Web.Business.Validators
{
    class OnBoardingCampaignPageValidation : IValidate<OnBoardingCampaignPage>
    {
        public IEnumerable<ValidationError> Validate(OnBoardingCampaignPage instance)
        {
            var validationErrorList = new List<ValidationError>();


            try
            {
                instance.Parameters.ConvertToObject<OnboardingParameters>();                
            }
            catch
            {
                validationErrorList.Add(new ValidationError
                {
                    ErrorMessage = "Parameters. Ej valid json.",
                    PropertyName = "Parameters",
                    ValidationType = ValidationErrorType.StorageValidation,
                    Severity = ValidationErrorSeverity.Error
                });
            }

            return validationErrorList;
        }
    }
}