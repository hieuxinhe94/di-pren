using System.Collections.Generic;
using Di.Common.Utils;
using EPiServer.Validation;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Partials.OrderFlow;

namespace Pren.Web.Business.Validators
{
    class OrderFlowCampaignPageValidation : IValidate<OrderFlowCampaignPage>
    {
        public IEnumerable<ValidationError> Validate(OrderFlowCampaignPage instance)
        {
            var validationErrorList = new List<ValidationError>();

            try
            {
                instance.Packages.ConvertToObject<List<PackageModel>>();
            }
            catch 
            {
                validationErrorList.Add(new ValidationError
                {
                    ErrorMessage = "Kampanjer. Ej valid json.",
                    PropertyName = "Kampanjer",
                    ValidationType = ValidationErrorType.StorageValidation,
                    Severity = ValidationErrorSeverity.Error
                });
            }

            return validationErrorList;
        }
    }
}