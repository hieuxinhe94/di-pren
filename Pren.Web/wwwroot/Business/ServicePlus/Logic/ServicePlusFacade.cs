using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Di.Common.Logging;
using Di.ServicePlus.RestApi.ResponseModels;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscribers;
using Di.Subscription.Logic.Package.Retrievers;
using Pren.Web.Business.Cache;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.ServicePlus.Models;

namespace Pren.Web.Business.ServicePlus.Logic
{
    public class ServicePlusFacade : IServicePlusFacade
    {
        private readonly ILogger _logService;
        private readonly IServicePlus _servicePlus;
        private readonly IObjectCache _objectCache;
        private readonly ISiteSettings _siteSettings;
        private readonly IPackageRetriever _packageRetriever;

        public ServicePlusFacade(
            IServicePlus servicePlus, 
            IObjectCache objectCache, 
            ISiteSettings siteSettings, IPackageRetriever packageRetriever)
        {
            _logService = new Log4NetLogger();
            _servicePlus = servicePlus;
            _objectCache = objectCache;
            _siteSettings = siteSettings;
            _packageRetriever = packageRetriever;
        }

        #region Users
        /// <summary>
        /// Creates a S+ user via REST API.
        /// </summary>
        /// <param name="email">Users e-mail</param>
        /// <param name="phone">Users phone</param>
        /// <param name="firstName">Users first name</param>
        /// <param name="lastName">Users last name</param>
        /// <param name="password">Users password (min-length = 6)</param>
        /// <param name="sendActivationMail">Default an activation mail is send to the created user. Set to false if no mail should be sent</param>
        /// <returns>User object for the created user, null if not successfully created</returns>
        public User CreateUser(string email, string phone, string firstName, string lastName, string password, bool sendActivationMail = true)
        {
            var createUserResponse = CreateServicePlusRequest(api => api.RestApi.Users.CreateUser(
                email,
                phone,
                firstName,
                lastName,
                password,
                UserType.CUSTOMER.ToString(),
                "5DuzcZz0j8u0zArSNzZgHO",//todo: kj brandid
                "0",//todo: kj const?
                Di.Common.Utils.DateTimeUtils.GetUnixTimeStampInMilliseconds(DateTime.Now).ToString(CultureInfo.InvariantCulture),
                GetSystemAccessToken()));

            if (createUserResponse == null || createUserResponse.CreatedUser == null)
            {
                return null;
            }

            var user = new User
            {
                Id = createUserResponse.CreatedUser.Id,
                Email = createUserResponse.CreatedUser.Email
            };

            if (sendActivationMail)
            {
                CreateServicePlusRequest(api => api.RestApi.Users.SendActivationPassword(user.Id, GetSystemAccessToken()));
            }

            return user;
        }

        public User GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var userResponse = CreateServicePlusRequest(api => api.RestApi.Users.GetUserById(id, GetSystemAccessToken()));

            if (userResponse == null || userResponse.User == null)
            {
                return null;
            }

            return new User
            {
                Id = userResponse.User.Id,
                Email = userResponse.User.Email,
                FirstName = userResponse.User.FirstName,
                LastName = userResponse.User.LastName
            };
        }

        public User GetUserByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var userResponse = CreateServicePlusRequest(api => api.RestApi.Users.GetUser(token));

            if (userResponse == null || userResponse.User == null)
            {
                return null;                
            }

            var user = new User
            {
                Id = userResponse.User.Id,
                Email = userResponse.User.Email,
                FirstName = userResponse.User.FirstName,
                LastName = userResponse.User.LastName
            };

            if (userResponse.User.ExternalUserIds == null)
            {
                return user;
            }

            var kayakUserId = userResponse.User.ExternalUserIds.FirstOrDefault(externaluserId => externaluserId.System.Equals(ExternalUserIdSystem.KAYAKBiz.ToString()));

            if (kayakUserId == null || string.IsNullOrEmpty(kayakUserId.Id))
            {
                return user;
            }

            user.KayakBizSubscriptionCustomerNumber = TryParseToLong(kayakUserId.Id);

            return user;
        }

        /// <summary>
        /// Tries to find S+ users by email
        /// </summary>
        /// <param name="email">Email of the user to search for</param>
        /// <returns>The first user found or null if no user found</returns>
        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var searchUserResponse = CreateServicePlusRequest(api => api.RestApi.Users.SearchUserByEmail(email, GetSystemAccessToken()));

            if (searchUserResponse == null || searchUserResponse.Users == null || !searchUserResponse.Users.Any())
            {
                return null;
            }

            var foundUser = searchUserResponse.Users.First();

            return new User
            {
                Id = foundUser.Id,
                Email = foundUser.Email,
            };
        }

        /// <summary>
        /// Tries to find S+ users by email
        /// </summary>
        /// <param name="email">Email of the user to search for</param>
        /// <returns>The first user found or null if no user found</returns>
        [Obsolete("EndPoint not completed by S+", true)]
        public User FindUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var usersResponse = CreateServicePlusRequest(api => api.RestApi.Users.FindUsersByEmail(email, GetSystemAccessToken(), _siteSettings.ServicePlusAppId));

            if (usersResponse == null || 
                (usersResponse.Users == null || !usersResponse.Users.Any()) &&
                usersResponse.BipAccount == null)
            {
                return null;
            }
           
            var foundUser =  usersResponse.Users == null ? usersResponse.BipAccount : usersResponse.Users.First();

            return new User
            {
                Id = foundUser.Id,
                Email = foundUser.Email,
                Message = usersResponse.Message
            };
        }

        public OrderFlowMessage GetOrderFlowMessage(string email, bool isLoggedIn)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var messageResponse = CreateServicePlusRequest(api => api.RestApi.Users.OrderFlowMessage(email, isLoggedIn, GetSystemAccessToken(), _siteSettings.ServicePlusBrandId));


            return new OrderFlowMessage
            {
                Message = messageResponse.Message,
                Header = messageResponse.Header,
                ForceLogin = messageResponse.ForceLogin
            };
        }

        public bool UpdateUser(string token, string firstName, string lastName, string phoneNumber, string email)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var userResponse = CreateServicePlusRequest(api => api.RestApi.Users.GetUser(token));

            if (userResponse == null || userResponse.User == null)
            {
                return false;                
            }

            var currentUser = userResponse.User;

            var currentUserJson = Newtonsoft.Json.JsonConvert.SerializeObject(currentUser);

            // Create a updated user object from the current user object that we can update the provided values on
            var updatedUser = currentUser;

            // Update user with provided values
            if (!string.IsNullOrEmpty(firstName))
            {
                updatedUser.FirstName = firstName;
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                updatedUser.LastName = lastName;
            }
            if (!string.IsNullOrEmpty(phoneNumber) && !Di.Common.Utils.PhoneNumberUtils.IsSamePhoneNumberExceptCountryPrefix(phoneNumber, currentUser.PhoneNumber, "+46"))
            {
                updatedUser.PhoneNumber = phoneNumber;
            }
            if (!string.IsNullOrEmpty(email))
            {
                updatedUser.Email = email;
            }

            var updatedUserJson = Newtonsoft.Json.JsonConvert.SerializeObject(updatedUser);

            // Compare current user wityh updated user, if they are equal no update is needed
            if (currentUserJson == updatedUserJson)
            {
                return true;
            }

            var updateUserResponse = CreateServicePlusRequest(api => api.RestApi.Users.UpdateUser(updatedUser, token));

            if (updateUserResponse == null || !updateUserResponse.HttpResponseCode.StartsWith("2"))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region BizSubscriptions
        /// <summary>
        /// bizsubscriptions/{id}/bizsubscribers
        /// </summary>
        /// <param name="bizSubscriptionId"></param>
        /// <returns></returns>
        public IEnumerable<BizSubscriber> GetActiveBizSubscribers(string bizSubscriptionId)
        {
            var subscribers = new List<BizSubscriber>();

            var activeSubscribersResponse = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetActiveBizSubscribers(bizSubscriptionId, GetSystemAccessToken()));

            if (activeSubscribersResponse == null ||
                activeSubscribersResponse.Subscribers == null ||
                !activeSubscribersResponse.Subscribers.Any())
            {
                return subscribers;
            }

            subscribers.AddRange(activeSubscribersResponse.Subscribers.Select(ConvertToBizSubscriber));

            return subscribers.OrderByDescending(s => s.Status).ThenBy(s => s.FirstName);
        }

        public IEnumerable<PendingBizSubscriber> GetPendingBizSubscribers(string bizSubscriptionId)
        {
            var subscribers = new List<PendingBizSubscriber>();

            var pendingSubscribersResponse = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetPendingBizSubscribers(bizSubscriptionId, GetSystemAccessToken()));

            if (pendingSubscribersResponse == null ||
                pendingSubscribersResponse.Subscribers == null ||
                !pendingSubscribersResponse.Subscribers.Any())
            {
                return subscribers;
            }

            subscribers.AddRange(pendingSubscribersResponse.Subscribers.Select(ConvertToPendingBizSubscriber));

            return subscribers;
        }

        public BizSubscriber GetBizSubscriberByInviteCode(string bizSubscriptionId, string inviteCode)
        {
            var invitedSubscriberResponse = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetInvitedBizSubscriber(bizSubscriptionId, inviteCode, GetSystemAccessToken()));

            if (invitedSubscriberResponse == null || invitedSubscriberResponse.Subscriber == null)
            {
                return null;
            }

            return new BizSubscriber
            {
                Email = invitedSubscriberResponse.Subscriber.Email
            };
        }

        public bool ActivateBizSubscriber(string bizSubscriptionId, string email)
        {
            return CreateServicePlusTrueOrFalseRequest(api => api.RestApi.BizSubscriptions.ActiveInviteByEmail(bizSubscriptionId, email, GetSystemAccessToken()));
        }

        public bool DeletePendingBizSubscriber(string bizSubscriptionId, string code)
        {
            return CreateServicePlusTrueOrFalseRequest(api => api.RestApi.BizSubscriptions.DeletePendingBizSubscriber(bizSubscriptionId, code, GetSystemAccessToken()));
        }

        public bool MarkActiveBizSubscriberForRemoval(string bizSubscriptionId, string userId, bool markForRemoval)
        {
            return CreateServicePlusTrueOrFalseRequest(api => api.RestApi.BizSubscriptions.MarkBizSubscriberForRemoval(bizSubscriptionId, userId, markForRemoval, GetSystemAccessToken()));
        }

        public bool InviteBizSubscriberByEmail(string bizSubscriptionId, string email)
        {
            return CreateServicePlusTrueOrFalseRequest(api => api.RestApi.BizSubscriptions.SendBizSubscriptionInvitationMail(bizSubscriptionId, email, "di.se", GetSystemAccessToken())); //todo: kj remove di.se and param
        }

        public bool RemindInvitedSubscriberByEmail(string bizSubscriptionId, string code)
        {
            return CreateServicePlusTrueOrFalseRequest(api => api.RestApi.BizSubscriptions.SendBizSubscriptionInvitationReminderMail(bizSubscriptionId, code, GetSystemAccessToken()));
        }

        public IEnumerable<BizSubscriptionDefinition> GetBizSubscriptionDefinitions()
        {
            var definitions = new List<BizSubscriptionDefinition>();

            var response = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetBizSubscriptionDefinitions(GetSystemAccessToken()));

            if (response == null || 
                response.BizSubscriptionDefinitions == null ||
                !response.BizSubscriptionDefinitions.Any())
            {
                return definitions;
            }

            definitions.AddRange(response.BizSubscriptionDefinitions                
                .Select(definition => new BizSubscriptionDefinition
            {
                Id = definition.Id,
                ExternalProductCode = definition.ExternalProductCode,
                MinQuantity = TryParseToInteger(definition.MinQuantity),
                MaxQuantity = TryParseToInteger(definition.MaxQuantity)
            }));

            return definitions;
        }

        public IEnumerable<BizSubscriptionDefinition> GetBizSubscriptionDefinitionsByProductId(string productId)
        {
            var definitions = new List<BizSubscriptionDefinition>();

            var response = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetBizSubscriptionDefinitionsByProductId(productId, GetSystemAccessToken()));

            if (response == null ||
                response.BizSubscriptionDefinitions == null ||
                !response.BizSubscriptionDefinitions.Any())
            {
                return definitions;
            }

            definitions.AddRange(response.BizSubscriptionDefinitions
                .Select(definition => new BizSubscriptionDefinition
                {
                    Id = definition.Id,
                    ExternalProductCode = definition.ExternalProductCode,
                    MinQuantity = TryParseToInteger(definition.MinQuantity),
                    MaxQuantity = TryParseToInteger(definition.MaxQuantity)
                }));

            return definitions;
        }

        /// <summary>
        /// Gets <see cref="BizSubscriptionDefinition"/> for specified id
        /// </summary>
        /// <param name="definitionId">The <see cref="BizSubscriptionDefinition"/> id</param>
        /// <returns><see cref="BizSubscriptionDefinition"/> for specified id or null if no definition found</returns>
        public BizSubscriptionDefinition GetBizSubscriptionDefinition(string definitionId)
        {
            var response = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetBizSubscriptionDefinition(definitionId, GetSystemAccessToken()));

            if (response == null || 
                response.BizSubscriptionDefinition == null ||
                !response.BizSubscriptionDefinition.Any())
            {
                return null;
            }

            var definition = response.BizSubscriptionDefinition.First();

            return new BizSubscriptionDefinition
            {
                Id = definition.Id,
                ExternalProductCode = definition.ExternalProductCode,
                MinQuantity = TryParseToInteger(definition.MinQuantity),
                MaxQuantity = TryParseToInteger(definition.MaxQuantity)
            };
        }

        public bool CreateBizSubscription(
            string orgName, 
            string businessSubscriptionDefinitionId, 
            string organizationNumber,
            string email, 
            string firstName, 
            string lastName, 
            string streetName, 
            string streetNumber, 
            string zipCode, 
            string city,
            string campNo, 
            string phoneNumber)
        {
            var brandId = "5DuzcZz0j8u0zArSNzZgHO"; //TODO: hämta från setting
            var country = "SE"; 

            var response = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.CreateBizSubscription( //_servicePlusApi.BizSubscriptions.CreateBizSubscription(
                brandId,
                orgName,
                businessSubscriptionDefinitionId,
                organizationNumber,
                email,
                firstName,
                lastName,
                streetName,
                streetNumber,
                zipCode,
                city,
                country,
                campNo,
                phoneNumber,
                GetSystemAccessToken()));

            if (response == null || response.BusinessSubscription == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a list of Business Subscription for specified user
        /// </summary>
        /// <param name="userId">Id of the User to get Business Subscriptions for</param>
        /// <returns>List of Business subscriptions, empty list if no subscriptions found</returns>
        public IEnumerable<BizSubscription> GetBizSubscriptions(string userId)
        {
            var bizSubscriptions = new List<BizSubscription>();

            var response = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetBizSubscriptions(userId, GetSystemAccessToken()));

            if (response == null ||
                response.BusinessSubscriptions == null ||
                !response.BusinessSubscriptions.Any())
            {
                return bizSubscriptions;
            }

            bizSubscriptions.AddRange(response.BusinessSubscriptions.Select(GetBizSubscription));

            return bizSubscriptions;
        }

        public BizSubscription GetBizSubscription(string bizSubscriptionId)
        {
            var bizSubscriptionResponse = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetBizSubscription(bizSubscriptionId, GetSystemAccessToken()));

            if (bizSubscriptionResponse == null ||
                bizSubscriptionResponse.BusinessSubscription == null)
            {
                return null;
            }

            return GetBizSubscription(bizSubscriptionResponse.BusinessSubscription);
        }

        public BizSubscriptionCount GetBizSubscriptionCount(string bizSubscriptionId)
        {
            var response = CreateServicePlusRequest(api => api.RestApi.BizSubscriptions.GetBizSubscriptionCount(bizSubscriptionId, GetSystemAccessToken()));

            if (response == null)
            {
                return null;
            }

            return new BizSubscriptionCount
            {
                ActiveSubscribers = TryParseToInteger(response.ActiveSubscribers),
                NumberOfSubscribers = TryParseToInteger(response.NumOfSubscribers),
                PendingSubscribers = TryParseToInteger(response.PendingSubscribers)
            };
        }
        #endregion

        #region Entitlements
        public Entitlement GetEntitlement(string entitlementId)
        {               
            var entitlementResponse = CreateServicePlusRequest(api => api.RestApi.Entitlements.GetEntitlement(entitlementId, GetSystemAccessToken()));

            if (entitlementResponse == null || entitlementResponse.Entitlement == null)
            {
                return null;
            }

            return new Entitlement
            {
                State = EntitlementStateToEnum(entitlementResponse.Entitlement.State),
                ValidFrom = Di.Common.Utils.DateTimeUtils.UnixTimeStampToDateTime(entitlementResponse.Entitlement.ValidFrom),
                ValidTo = Di.Common.Utils.DateTimeUtils.UnixTimeStampToDateTime(entitlementResponse.Entitlement.ValidTo)
            };
        }

        public IEnumerable<Entitlement> GetEntitlements(string userId)
        {
            var entitlements = new List<Entitlement>();

            var entitlementsResponse = CreateServicePlusRequest(api => api.RestApi.Entitlements.GetEntitlements(userId, GetSystemAccessToken()));

            if (entitlementsResponse == null || entitlementsResponse.Entitlements == null || !entitlementsResponse.Entitlements.Any())
            {
                return entitlements;
            }

            entitlements.AddRange(entitlementsResponse.Entitlements.Select(e => new Entitlement
            {
                State = EntitlementStateToEnum(e.State),
                ValidFrom = Di.Common.Utils.DateTimeUtils.UnixTimeStampToDateTime(e.ValidFrom),
                ValidTo = Di.Common.Utils.DateTimeUtils.UnixTimeStampToDateTime(e.ValidTo),
                SubscriberId = TryParseToLong(e.ExternalSubscriberId)
            }));

            return entitlements;
        }

        public IEnumerable<ExternalId> GetExternalIds(string token)
        {
            var externalIds = new List<ExternalId>();

            var externalIdsResponse = _servicePlus.RestApi.Entitlements.GetExternalIds(token);

            if (externalIdsResponse == null || externalIdsResponse.ExternalIds == null || !externalIdsResponse.ExternalIds.Any())
            {
                return externalIds;
            }

            externalIds.AddRange(externalIdsResponse.ExternalIds.Select(eId => new ExternalId
            {
                ExternalProductId = eId.ExternalProductId,
                ExternalSubscriberId = eId.ExternalSubscriberId
            }));

            return externalIds;
        }

        public IEnumerable<ExternalId> GetFilteredExternalIds(string token)
        {
            var externalIds = GetExternalIds(token).ToList();

            return !externalIds.Any() 
                ? externalIds 
                : externalIds.Where(externalId => _packageRetriever.GetAllProductPackageIds().Any(productIdToInclude => productIdToInclude == externalId.ExternalProductId)).ToList();
        }

        public bool VerifyEntitlement(string externalResourceId, string token)
        {
            var verifyEntitlementResponse = CreateServicePlusRequest(api => api.RestApi.Entitlements.VerifyEntitlement(externalResourceId, token));

            if (verifyEntitlementResponse == null)
            {
                return false;
            }

            return verifyEntitlementResponse.Entitled;
        }

        public bool ImportEntitlement(string userId, long externalSubscriberId, long externalSubscriptionId, string paperCode, string productNumber)
            {
            var productId = _siteSettings.GetServicePlusProductId(paperCode, productNumber);

            var importEntitlementResponse =
                CreateServicePlusRequest(
                    api =>
                        api.RestApi.Entitlements.ImportEntitlement(productId, userId, externalSubscriberId,
                            externalSubscriptionId, GetSystemAccessToken()));

            return importEntitlementResponse != null && importEntitlementResponse.HttpResponseCode.StartsWith("2");
        }

        #endregion

        #region Offers

        /// <summary>
        /// Creates or updates order on user.
        /// </summary>
        /// <param name="userToken">Token for the user to update</param>
        /// <param name="offerOrigin">Origin of offer</param>
        /// <returns>True if succefully updated</returns>
        public bool CreateOrUpdateOffer(string userToken, string offerOrigin)
        {
            var user = GetUserByToken(userToken);

            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return false;
            }

            var createOrUpdateOfferResponse = CreateServicePlusRequest(api => api.RestApi.Offers.CreateOrUpdateOffer(
                userId: user.Id, 
                brandId: _siteSettings.ServicePlusBrandId, 
                productId: _siteSettings.ServicePlusCreateOrUpdateOfferProductId,
                token: GetSystemAccessToken(),
                offerType: "user_defined",
                subscriptionLenght: (new TimeSpan(7, 0, 0, 0).TotalSeconds * 1000).ToString(CultureInfo.InvariantCulture), 
                forceDisplayed: false,
                accepted: true,
                offerOrigin: offerOrigin
                ));

            return createOrUpdateOfferResponse != null && createOrUpdateOfferResponse.HttpResponseCode.StartsWith("2");
        }

        public bool HasBizSubscription(string userId)
        {
            var bizSubscriptions = GetBizSubscriptions(userId);

            return bizSubscriptions != null && bizSubscriptions.Any();
        }

        #endregion

        #region Private methods
        private bool CreateServicePlusTrueOrFalseRequest<TSResponse>(Func<IServicePlus, TSResponse> apiRequest) where TSResponse : ResponseBase
        {
            var response = CreateServicePlusRequest(apiRequest);

            if (response == null)
            {
                return false;
            }

            var responseBase = (ResponseBase)response;

            return responseBase.Result == "true";
        }

        private TSResponse CreateServicePlusRequest<TSResponse>(Func<IServicePlus, TSResponse> apiRequest) where TSResponse : ResponseBase
        {
            try
            {
                var response = apiRequest.Invoke(_servicePlus);

                var responseBase = (ResponseBase)response;

                if (!responseBase.HttpResponseCode.StartsWith("2"))
                {
                    _logService.Log("S+ Request failed " + apiRequest.Method + " " + apiRequest.Target + " - " + string.Format("ErrorCode: {0}, ErrorMessage: {1}", responseBase.ErrorCode, responseBase.ErrorMessage), LogLevel.Error, typeof(ServicePlusFacade));
                }

                return response;
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "S+ Request exception " + apiRequest.Method + " " + apiRequest.Target + " - ", LogLevel.Error, typeof(ServicePlusFacade));
                return null;
            }

        }

        private BizSubscriber ConvertToBizSubscriber(Subscriber subscriber)
        {
            return new BizSubscriber
            {
                Email = subscriber.Email,
                UserId = subscriber.SubUserId,
                Active = true,
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                RemovalDateString = !string.IsNullOrEmpty(subscriber.Removal) ? Di.Common.Utils.DateTimeUtils.UnixTimeStampToDateTime(double.Parse(subscriber.Removal)).ToString("yyyy-MM-dd") : string.Empty,
                Status = subscriber.Status
            };
        }

        private PendingBizSubscriber ConvertToPendingBizSubscriber(PendingSubscriber subscriber)
        {
            return new PendingBizSubscriber
            {
                Email = subscriber.Email,
                UserId = subscriber.SubUserId,
                Active = false,
                FirstName = "",
                LastName = "",
                Code = subscriber.InviteCode
            };
        }

        private BizSubscription GetBizSubscription(Di.ServicePlus.RestApi.ResponseModels.BizSubscriptions.BusinessSubscription businessSubscription)
        {
            return new BizSubscription
            {
                Id = businessSubscription.Id,
                DefinitionId = businessSubscription.BusinessSubscriptionDefinitionId,
                CompanyName = businessSubscription.OrgName,
                EntitlementId = businessSubscription.EntitlementId
            };
        }

        private int TryParseToInteger(string number)
        {
            int result;
            int.TryParse(number, out result);
            return result;
        }

        private long TryParseToLong(string number)
        {
            long result;
            long.TryParse(number, out result);
            return result;
        }

        private EntitlementState EntitlementStateToEnum(string state)
        {
            switch (state)
            {
                case "VALID":
                    return EntitlementState.Valid;
                case "INVALID":
                    return EntitlementState.Invalid;
                case "SYNCING":
                    return EntitlementState.Syncing;
                default:
                    return EntitlementState.Undefined;
            }
        }

        private string GetSystemAccessToken()
        {
            const string cacheKey = "s+systemtoken";

            var servicePlusToken = (ServicePlusToken)_objectCache.GetFromCache(cacheKey);

            if (servicePlusToken != null && servicePlusToken.Expires >= DateTime.Now)
            {
                return servicePlusToken.AccessToken;
            }

            var systemAccessToken = _servicePlus.RestApi.OAuth.GetSystemUserAccessToken(_siteSettings.ServicePlusClientId, _siteSettings.ServicePlusClientSecret, "client_credentials");
            
            servicePlusToken = new ServicePlusToken
            {
                AccessToken = systemAccessToken.AccessToken,
                Expires = DateTime.Now.AddSeconds(double.Parse(systemAccessToken.ExpiresIn))
            };

            _objectCache.AddToCache(cacheKey, servicePlusToken);

            return servicePlusToken.AccessToken;
        }

        private class ServicePlusToken
        {
            public string AccessToken { get; set; }
            public DateTime Expires { get; set; }
        }
        #endregion
    }
}