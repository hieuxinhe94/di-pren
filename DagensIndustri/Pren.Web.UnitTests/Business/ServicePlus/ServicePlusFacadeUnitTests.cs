using System;
using System.Linq;
using Di.ServicePlus.RestApi.ResponseModels.User;
using Di.Subscription.Logic.Package.Retrievers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.UnitTests.Fakes.ServicePlus;

namespace Pren.Web.UnitTests.Business.ServicePlus
{
    [TestClass]
    public class ServicePlusFacadeUnitTests
    {
        private IServicePlusFacade GetServicePlusFacade(IServicePlus servicePlus = null)
        {
            var servicePlusFacade = new ServicePlusFacade(
                servicePlus ?? FakeServicePlusFactory.FakeServicePlus().Object,
                new Mock<Web.Business.Cache.IObjectCache>().Object, 
                new Mock<ISiteSettings>().Object,
                new Mock<IPackageRetriever>().Object
            );

            return servicePlusFacade;
        }

        [TestMethod]
        public void GetUserByToken_TokenIsEmptyString_ReturnsNull()
        {
            var servicePlusFacade = GetServicePlusFacade();

            var user = servicePlusFacade.GetUserByToken(string.Empty);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByToken_TokenIsNull_ReturnsNull()
        {
            var servicePlusFacade = GetServicePlusFacade();

            var user = servicePlusFacade.GetUserByToken(null);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByToken_WithExpiredToken_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUserThatReturnsExpiredTokenResponse());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByToken("dummytoken");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByToken_UserExistInServicePlus_ReturnsUser()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUserThatReturnsValidUserResponse());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByToken("dummytoken");

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void GetUserByToken_UserExistInServicePlus_ReturnsUserWithPropertiesSetFromResponse()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUserThatReturnsValidUserResponse());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByToken("dummytoken");

            var userResponse = UsersApiResponseFactory.FakeUserResponseValidUser();

            Assert.AreEqual(userResponse.User.Id, user.Id, "Id not set on User");
            Assert.AreEqual(userResponse.User.Email, user.Email, "Email not set on User");
            Assert.AreEqual(userResponse.User.FirstName, user.FirstName, "FirstName not set on User");
            Assert.AreEqual(userResponse.User.LastName, user.LastName, "LastName not set on User");
        }

        [TestMethod]
        public void GetUserByToken_UserExistInServicePlusAndHasBizSubscription_ReturnsUserWithkayakBizSubscriptionId()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUserThatReturnsValidUserResponseWithBizSubscription());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByToken("dummytoken");

            Assert.IsTrue(user.KayakBizSubscriptionCustomerNumber > 0, "KayakBizSubscriptionCustomerNumber not set on User");
        }

        [TestMethod]
        public void GetUserByToken_ServicePlusUserApiThrowsException_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUserThatThrowsException());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByToken("dummytoken");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByToken_ServicePlusUserApiReturnsNull_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(null));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByToken("dummytoken");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_EmailIsEmptyString_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus();

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail(string.Empty);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_EmailIsNull_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus();

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail(null);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_InvalidSystemToken_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsFaultyTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithSearchUserByEmailThatReturnsUserFoundSearchUserResponse());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_UserNotFoundInServicePlus_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithSearchUserByEmailThatReturnsUserNotFoundSearchUserResponse());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_ServicePlusOAuthApiThrowsExceptions_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatThrowsException());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_ServicePlusOAuthApiReturnsNull_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessToken(null));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_ServicePlusUserApiThrowsExceptions_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithSearchUserByEmailThatThrowsException());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_ServicePlusUserApiReturnsNull_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithSearchUserByEmail(null));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_UserFoundInServicePlus_ReturnsUser()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithSearchUserByEmailThatReturnsUserFoundSearchUserResponse());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void GetUserByEmail_UserFoundInServicePlus_ReturnsUserWithPropertiesSetFromResponse()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithSearchUserByEmailThatReturnsUserFoundSearchUserResponse());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            var userResponse = UsersApiResponseFactory.FakeSearchUserResponseUserFound();

            Assert.AreEqual(userResponse.Users.First().Id, user.Id);
            Assert.AreEqual(userResponse.Users.First().Email, user.Email);
        }

        [TestMethod]
        public void GetUserByEmail_MultipleUsersFoundInServicePlus_ReturnsFirstUserInResponseList()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithSearchUserByEmailThatReturnsMultipleUsersFoundSearchUserResponse());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var user = servicePlusFacade.GetUserByEmail("dummyemail@dummy.email");

            var userResponse = UsersApiResponseFactory.FakeSearchUserResponseUserFound();

            Assert.AreEqual(userResponse.Users.First().Id, user.Id);
            Assert.AreEqual(userResponse.Users.First().Email, user.Email);
        }

        [TestMethod]
        public void CreateUser_SuccessFullyCreatedUser_ReturnsCreatedUser()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithCreateUserThatReturnsSuccessfullyCreatedUser());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var createdUser = servicePlusFacade.CreateUser(
                "fake@dummy.email",
                "123456789",
                "fakefirstname",
                "fakelastname,",
                "fakepw", 
                false);

            Assert.IsNotNull(createdUser);
        }

        [TestMethod]
        public void CreateUser_ShouldSendActivationMail_SendActivationPasswordIsCalled()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithCreateUserThatReturnsSuccessfullyCreatedUser());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var createdUser = servicePlusFacade.CreateUser(
                "fake@dummy.email",
                "123456789",
                "fakefirstname",
                "fakelastname,",
                "fakepw",
                true);

            Mock.Get(fakeServicePlus.Object.RestApi.Users).Verify(x => x.SendActivationPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void CreateUser_ShouldNotSendActivationMail_SendActivationPasswordIsNotCalled()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithCreateUserThatReturnsSuccessfullyCreatedUser());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var createdUser = servicePlusFacade.CreateUser(
                "fake@dummy.email",
                "123456789",
                "fakefirstname",
                "fakelastname,",
                "fakepw",
                false);

            Mock.Get(fakeServicePlus.Object.RestApi.Users).Verify(x => x.SendActivationPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void CreateUser_ServicePlusUserApiThrowsException_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithCreateUserThatThrowsException());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var createdUser = servicePlusFacade.CreateUser(
                "fake@dummy.email",
                "123456789",
                "fakefirstname",
                "fakelastname,",
                "fakepw",
                false);

            Assert.IsNull(createdUser);
        }

        [TestMethod]
        public void CreateUser_ServicePlusOAuthApiThrowsException_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatThrowsException());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var createdUser = servicePlusFacade.CreateUser(
                "fake@dummy.email",
                "123456789",
                "fakefirstname",
                "fakelastname,",
                "fakepw",
                false);

            Assert.IsNull(createdUser);
        }

        [TestMethod]
        public void UpdateUser_NewFirstName_UpdatesFirstName()
        {
            var currentUserResponse = UsersApiResponseFactory.FakeUserResponseValidUser();
            var userToUpdate = UsersApiResponseFactory.FakeUserResponseValidUser().User;
            userToUpdate.FirstName = "NewFirstName";

            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(currentUserResponse)
                    .WithUpdateUser(UsersApiResponseFactory.FakeUpdateUserResponseValidUser(userToUpdate)));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var userUpdated = servicePlusFacade.UpdateUser(
                "usertoken", 
                userToUpdate.FirstName, 
                userToUpdate.LastName,
                userToUpdate.PhoneNumber, 
                userToUpdate.Email);

            Assert.IsTrue(userUpdated);

            Mock.Get(fakeServicePlus.Object.RestApi.Users).UpdateUserSetup()
                .Callback((User updatedUser, string token) => Assert.AreEqual(userToUpdate.FirstName, updatedUser.FirstName));

            Mock.Get(fakeServicePlus.Object.RestApi.Users)
                .Verify(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_NewFirstNameIsSameAsExistsingFirstNameExceptCasing_UpdatesFirstName()
        {
            var currentUserResponse = UsersApiResponseFactory.FakeUserResponseValidUser();
            var userToUpdate = UsersApiResponseFactory.FakeUserResponseValidUser().User;
            userToUpdate.FirstName = currentUserResponse.User.FirstName.ToUpper();

            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(currentUserResponse)
                    .WithUpdateUser(UsersApiResponseFactory.FakeUpdateUserResponseValidUser(userToUpdate)));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var userUpdated = servicePlusFacade.UpdateUser(
                "usertoken",
                userToUpdate.FirstName,
                userToUpdate.LastName,
                userToUpdate.PhoneNumber,
                userToUpdate.Email);

            Assert.IsTrue(userUpdated);

            Mock.Get(fakeServicePlus.Object.RestApi.Users).UpdateUserSetup()
                .Callback((User updatedUser, string token) => Assert.AreEqual(userToUpdate.FirstName, updatedUser.FirstName));

            Mock.Get(fakeServicePlus.Object.RestApi.Users)
                .Verify(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_NewLastName_UpdatesLastName()
        {
            var currentUserResponse = UsersApiResponseFactory.FakeUserResponseValidUser();
            var userToUpdate = UsersApiResponseFactory.FakeUserResponseValidUser().User;
            userToUpdate.LastName = "NewLastName";

            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(currentUserResponse)
                    .WithUpdateUser(UsersApiResponseFactory.FakeUpdateUserResponseValidUser(userToUpdate)));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var userUpdated = servicePlusFacade.UpdateUser(
                "usertoken",
                userToUpdate.FirstName,
                userToUpdate.LastName,
                userToUpdate.PhoneNumber,
                userToUpdate.Email);

            Assert.IsTrue(userUpdated);

            Mock.Get(fakeServicePlus.Object.RestApi.Users).UpdateUserSetup()
                .Callback((User updatedUser, string token) => Assert.AreEqual(userToUpdate.LastName, updatedUser.LastName));

            Mock.Get(fakeServicePlus.Object.RestApi.Users)
                .Verify(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_NewLastNameIsSameAsExistsingLastNameExceptCasing_UpdatesLastName()
        {
            var currentUserResponse = UsersApiResponseFactory.FakeUserResponseValidUser();
            var userToUpdate = UsersApiResponseFactory.FakeUserResponseValidUser().User;
            userToUpdate.LastName = currentUserResponse.User.LastName.ToUpper();

            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(currentUserResponse)
                    .WithUpdateUser(UsersApiResponseFactory.FakeUpdateUserResponseValidUser(userToUpdate)));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var userUpdated = servicePlusFacade.UpdateUser(
                "usertoken",
                userToUpdate.FirstName,
                userToUpdate.LastName,
                userToUpdate.PhoneNumber,
                userToUpdate.Email);

            Assert.IsTrue(userUpdated);

            Mock.Get(fakeServicePlus.Object.RestApi.Users).UpdateUserSetup()
                .Callback((User updatedUser, string token) => Assert.AreEqual(userToUpdate.LastName, updatedUser.LastName));

            Mock.Get(fakeServicePlus.Object.RestApi.Users)
                .Verify(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_NewPhoneNumber_UpdatesPhoneNumber()
        {
            var currentUserResponse = UsersApiResponseFactory.FakeUserResponseValidUser();
            var userToUpdate = UsersApiResponseFactory.FakeUserResponseValidUser().User;
            userToUpdate.PhoneNumber = "0708747041";

            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(currentUserResponse)
                    .WithUpdateUser(UsersApiResponseFactory.FakeUpdateUserResponseValidUser(userToUpdate)));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var userUpdated = servicePlusFacade.UpdateUser(
                "usertoken",
                userToUpdate.FirstName,
                userToUpdate.LastName,
                userToUpdate.PhoneNumber,
                userToUpdate.Email);

            Assert.IsTrue(userUpdated);

            Mock.Get(fakeServicePlus.Object.RestApi.Users).UpdateUserSetup()
                .Callback((User updatedUser, string token) => Assert.AreEqual(userToUpdate.PhoneNumber, updatedUser.PhoneNumber));

            Mock.Get(fakeServicePlus.Object.RestApi.Users)
                .Verify(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_NewPhoneNumberIsSameAsExistingPhoneNumberExcept0InsteadOfPlus46_DoesNotUpdatePhoneNumber()
        {
            var currentUserResponse = UsersApiResponseFactory.FakeUserResponseValidUser();
            currentUserResponse.User.PhoneNumber = "+46708747041";
            var userToUpdate = UsersApiResponseFactory.FakeUserResponseValidUser().User;
            userToUpdate.PhoneNumber = "0708747041";

            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(currentUserResponse));
                    

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var userUpdated = servicePlusFacade.UpdateUser(
                "usertoken",
                userToUpdate.FirstName,
                userToUpdate.LastName,
                userToUpdate.PhoneNumber,
                userToUpdate.Email);

            Mock.Get(fakeServicePlus.Object.RestApi.Users)
                .Verify(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void UpdateUser_NewEmail_UpdatesEmail()
        {
            var currentUserResponse = UsersApiResponseFactory.FakeUserResponseValidUser();
            var userToUpdate = UsersApiResponseFactory.FakeUserResponseValidUser().User;
            userToUpdate.Email = "new@fake.email";

            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(currentUserResponse)
                    .WithUpdateUser(UsersApiResponseFactory.FakeUpdateUserResponseValidUser(userToUpdate)));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var userUpdated = servicePlusFacade.UpdateUser(
                "usertoken",
                userToUpdate.FirstName,
                userToUpdate.LastName,
                userToUpdate.PhoneNumber,
                userToUpdate.Email);

            Assert.IsTrue(userUpdated);

            Mock.Get(fakeServicePlus.Object.RestApi.Users).UpdateUserSetup()
                .Callback((User updatedUser, string token) => Assert.AreEqual(userToUpdate.Email, updatedUser.Email));

            Mock.Get(fakeServicePlus.Object.RestApi.Users)
                .Verify(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_NoChangedFields_DoesNotUpdate()
        {
            var currentUserResponse = UsersApiResponseFactory.FakeUserResponseValidUser();
            var userToUpdate = UsersApiResponseFactory.FakeUserResponseValidUser().User;

            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithUsers(FakeServicePlusFactory.FakeUsersApi()
                    .WithGetUser(currentUserResponse));


            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var userUpdated = servicePlusFacade.UpdateUser(
                "usertoken",
                userToUpdate.FirstName,
                userToUpdate.LastName,
                userToUpdate.PhoneNumber,
                userToUpdate.Email);

            Mock.Get(fakeServicePlus.Object.RestApi.Users)
                .Verify(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementExistsAndIsValid_ReturnsEntitlementWithStateValid()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementWithStateValid());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.AreEqual(EntitlementState.Valid, entitlement.State);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementExistsAndIsInvalid_ReturnsEntitlementWithStateInvalid()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementWithStateInvalid());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.AreEqual(EntitlementState.Invalid, entitlement.State);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementExistsAndIsSyncing_ReturnsEntitlementWithStateSyncing()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementWithStateSyncing());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.AreEqual(EntitlementState.Syncing, entitlement.State);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementExistsAndHasUnhandledState_ReturnsEntitlementWithStateUndefined()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementWithUnhandledState());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.AreEqual(EntitlementState.Undefined, entitlement.State);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementExistsAndHasValidToDateInPast_ReturnsEntitlementWithValidToDateInPast()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementWithValidToDateInPast());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsTrue(entitlement.ValidTo < DateTime.Now);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementExistsAndHasValidToDateInFuture_ReturnsEntitlementWithValidToDateInFuture()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementWithValidToDateInFuture());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsTrue(entitlement.ValidTo > DateTime.Now);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementExistsAndHasValidFromDateInPast_ReturnsEntitlementWithValidFromDateInPast()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementWithValidFromDateInPast());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsTrue(entitlement.ValidFrom < DateTime.Now);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementExistsAndHasValidFromDateInFuture_ReturnsEntitlementWithValidFromDateInFuture()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementWithValidFromDateInFuture());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsTrue(entitlement.ValidFrom > DateTime.Now);
        }

        [TestMethod]
        public void GetEntitlement_EntitlementNotFound_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatReturnsEntitlementNotFound());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsNull(entitlement);
        }

        [TestMethod]
        public void GetEntitlement_ServicePlusEntitlementsApiThrowsException_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlementThatThrowsException());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsNull(entitlement);
        }

        [TestMethod]
        public void GetEntitlement_ServicePlusEntitlementsApiReturnsNull_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatReturnsValidTokenResponse())
                .WithEntitlements(FakeServicePlusFactory.FakeEntitlementsApi()
                    .WithGetEntitlement(null));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsNull(entitlement);
        }

        [TestMethod]
        public void GetEntitlement_ServicePlusOAuthApiThrowsException_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessTokenThatThrowsException());

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsNull(entitlement);
        }

        [TestMethod]
        public void GetEntitlement_ServicePlusOAuthApiReturnsNull_ReturnsNull()
        {
            var fakeServicePlus = FakeServicePlusFactory.FakeServicePlus()
                .WithOAuth(FakeServicePlusFactory.FakeOAuthApi()
                    .WithGetSystemUserAccessToken(null));

            var servicePlusFacade = GetServicePlusFacade(servicePlus: fakeServicePlus.Object);

            var entitlement = servicePlusFacade.GetEntitlement("fakeentitlementid");

            Assert.IsNull(entitlement);
        }
    }
}
