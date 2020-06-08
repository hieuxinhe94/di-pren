using System.Collections.Generic;
using System.Linq;
using Di.Common.Logging;
using Di.Subscription.Logic.Parameters;
using EPiServer.Validation;
using Pren.Web.Models.Pages;

namespace Pren.Web.Business.Validators
{
    class CampaignTargetGroupValidation : IValidate<CampaignPage>
    {
        private readonly ILogger _logService;
        private readonly IParametersHandler _parametersHandler;

        public CampaignTargetGroupValidation(ILogger logService, IParametersHandler parametersHandler)
        {
            _logService = logService;
            _parametersHandler = parametersHandler;
        }

        public IEnumerable<ValidationError> Validate(CampaignPage instance)
        {
            var validationErrorList = new List<ValidationError>();

            if (string.IsNullOrEmpty(instance.TargetGroup) && string.IsNullOrEmpty(instance.TargetGroupMobile))
            {
                return validationErrorList;
            }

            if (!IsValidTargetGroup(instance.TargetGroup))
            {
                validationErrorList.Add(GetTargetGroupValidationError("Felaktig målgrupp desktop", "TargetGroup"));
            }
            if (!IsValidTargetGroup(instance.TargetGroupMobile))
            {
                validationErrorList.Add(GetTargetGroupValidationError("Felaktig målgrupp mobil", "TargetGroupMobile"));
            }

            return validationErrorList;
        }

        private bool IsValidTargetGroup(string targetGroup)
        {
            if (string.IsNullOrEmpty(targetGroup))
            {
                return true;
            }

            var targetGroups = _parametersHandler.ParametersRetriever.GetAllTargetGroups().ToList();

            if (targetGroups.Any())
            {
                return targetGroups.Any(t => t.TargetGroupName.Equals(targetGroup));
            }

            _logService.Log(
                "targetgroups was null when trying to validate target groups on CampaignPage. Targetgroup " +
                targetGroup + " treated as valid", LogLevel.Info, typeof(CampaignTargetGroupValidation));
            return true;
        }

        private ValidationError GetTargetGroupValidationError(string errorMessage, string propertyName)
        {
            return new ValidationError
            {
                ErrorMessage = errorMessage,
                PropertyName = propertyName,
                ValidationType = ValidationErrorType.StorageValidation,
                Severity = ValidationErrorSeverity.Error
            };
        }
    }
}
