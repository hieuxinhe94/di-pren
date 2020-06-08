using System.Collections.Generic;
using EPiServer.Validation;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Business.Validators
{
    class CampaignTrialValidation : IValidate<CampaignPeriodBlock>
    {
        public IEnumerable<ValidationError> Validate(CampaignPeriodBlock instance)
        {
            var validationErrorList = new List<ValidationError>();

            if (instance.IsTrial && instance.IsTrialFree)
            {
                validationErrorList.Add(new ValidationError
                {
                    ErrorMessage = "Du kan bara ange en prisgrupp, 42 eller 43. Inte båda.",
                    PropertyName = "IsTrial",
                    ValidationType = ValidationErrorType.StorageValidation,
                    Severity = ValidationErrorSeverity.Error
                });
            }

            return validationErrorList;
        }
    }
}
