using System;
using System.Collections.Generic;
using Di.Common.Logging;
using Di.Common.Utils;
using Di.Common.Utils.Authentication;
using Di.Common.WebRequests;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Student.ResponseModels;

namespace Pren.Web.Business.Student
{
    public class StudentHandler : IStudentHandler
    {
        private readonly ISiteSettings _siteSettings;
        private readonly IRequestService _requestService;
        private readonly IDetectionHandler _detection;
        private readonly ILogger _logger;

        public StudentHandler(ISiteSettings siteSettings, IRequestService requestService, IDetectionHandler detection, ILogger logger)
        {
            _siteSettings = siteSettings;
            _requestService = requestService;
            _detection = detection;
            _logger = logger;
        }

        /// <summary>
        /// Verify student by social security number.
        /// </summary>
        /// <param name="socialSecurityNumber">YYYYMMDDXXXX</param>
        /// <returns>true=full time student, false=not full time student</returns>
        public bool IsStudent(string socialSecurityNumber)
        {
            try
            {
                if (!_detection.IsNumeric(socialSecurityNumber))
                    return false;

                var basicAuthenticationHeader = AuthenticationUtil.GetBasicAuthenticationHeader(_siteSettings.StudentVerificationUserName, _siteSettings.StudentVerificationPassword);

                var response = _requestService.CreateGetRequestWithHeaderParams(
                    _siteSettings.StudentVerificationUrl + "?socialsecuritynumber=" + socialSecurityNumber,
                    new Dictionary<string, string> { { basicAuthenticationHeader.Key, basicAuthenticationHeader.Value } });

                var student = response.ConvertToObject<StudentResponse>();

                return student.ReturnValue == "VALID";
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "IsStudent failed", LogLevel.Error, typeof(StudentHandler));

                return false; //TODO: eller ska de godkännas ifall studentkollen skiter sig? TKM
            }
        }

    }
}
