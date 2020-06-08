using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Campaign;
using EPiServer.Validation;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Business.Validators
{
    class CampaignSubsKindValidation : IValidate<CampaignPeriodBlock>
    {
        private readonly ICampaignHandler _campaignHandler;

        public CampaignSubsKindValidation(ICampaignHandler campaignHandler)
        {
            _campaignHandler = campaignHandler;
        }


        public IEnumerable<ValidationError> Validate(CampaignPeriodBlock instance)
        {
            var validationErrorList = new List<ValidationError>();
            var subsKind = instance.SubsKind;

            // Property not set, handle as valid
            if (subsKind == null || subsKind == "0") return validationErrorList;

            // Make sure selected campaign id is valid for selected subsKind
            ValidateSubsKind(validationErrorList, instance.CampaignCardAndInvoice, subsKind);
            ValidateSubsKind(validationErrorList, instance.CampaignAutogiro, subsKind);

            return validationErrorList;
        }

        /// <summary>
        /// Returns if campaign has support for selected subsKind.
        /// </summary>
        /// <param name="errorList"></param>
        /// <param name="campIdRaw">PropertyData. Can contain both campId and campNo separated by pipe (|)</param>
        /// <param name="subsKind">Subskind to validate</param>
        private void ValidateSubsKind(ICollection<ValidationError> errorList, string campIdRaw, string subsKind)
        {            
            var campId = campIdRaw?.Split('|')[0];

            // if campaign property not set, handle as valid
            if (string.IsNullOrEmpty(campId)) return;

            var validate = new Action<ICollection<ValidationError>, bool>(
                (validationErrorList, valid) =>
                {
                    if (!valid)
                    {
                        errorList.Add(new ValidationError
                        {
                            ErrorMessage = "KampanjId " + campIdRaw + ", har inte stöd för subsKind " + subsKind,
                            PropertyName = "SubsKind",
                            ValidationType = ValidationErrorType.StorageValidation,
                            Severity = ValidationErrorSeverity.Error
                        });
                    }
                }
                );

            var campaign = _campaignHandler.GetCampaign(campId);

            switch (subsKind)
            {
                case "01":
                    validate(errorList, campaign.SubsKindOngoingEnabled);
                    return;
                case "02":
                    validate(errorList, campaign.SubsKindTimedEnabled);
                    return;
                default:
                    return;
            }
        }

    }
}
