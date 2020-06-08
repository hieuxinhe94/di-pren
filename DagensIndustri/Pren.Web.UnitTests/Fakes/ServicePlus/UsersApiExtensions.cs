using System;
using Di.ServicePlus.RestApi.Requests.Users;
using Di.ServicePlus.RestApi.ResponseModels.User;
using Moq;
using Moq.Language.Flow;

namespace Pren.Web.UnitTests.Fakes.ServicePlus
{
    static class UsersApiExtensions
    {
        #region CreateUser

        private static ISetup<IUsers, CreateUserResponse> CreateUserSetup(this Mock<IUsers> users)
        {
            return users.Setup(x => x.CreateUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()));
        }

        public static Mock<IUsers> WithCreateUser(this Mock<IUsers> users, CreateUserResponse createUserResponseToReturn)
        {
            users.CreateUserSetup()
                .Returns(createUserResponseToReturn);

            return users;
        }

        public static Mock<IUsers> WithCreateUserThatReturnsSuccessfullyCreatedUser(this Mock<IUsers> users)
        {
            users.WithCreateUser(UsersApiResponseFactory.FakeCreateUserResponseSuccessfull());

            return users;
        }

        public static Mock<IUsers> WithCreateUserThatThrowsException(this Mock<IUsers> users)
        {
            users.CreateUserSetup()
                .Throws(new Exception("FakeException"));

            return users;
        }

        #endregion
        
        #region GetUser

        private static ISetup<IUsers, UserResponse> CreateGetUserSetup(this Mock<IUsers> users)
        {
            return users.Setup(x => x.GetUser(It.IsAny<string>()));
        }

        public static Mock<IUsers> WithGetUser(this Mock<IUsers> users, UserResponse userResponseToReturn)
        {
            users.CreateGetUserSetup()
                .Returns(userResponseToReturn);

            return users;
        }

        public static Mock<IUsers> WithGetUserThatReturnsExpiredTokenResponse(this Mock<IUsers> users)
        {
            users.WithGetUser(UsersApiResponseFactory.FakeUserResponseExpiredToken());

            return users;
        }

        public static Mock<IUsers> WithGetUserThatReturnsValidUserResponse(this Mock<IUsers> users)
        {
            users.WithGetUser(UsersApiResponseFactory.FakeUserResponseValidUser());

            return users;
        }

        public static Mock<IUsers> WithGetUserThatReturnsValidUserResponseWithBizSubscription(this Mock<IUsers> users)
        {
            users.WithGetUser(UsersApiResponseFactory.FakeUserResponseValidUserWithBizSubscription());

            return users;
        }

        public static Mock<IUsers> WithGetUserThatThrowsException(this Mock<IUsers> users)
        {
            users.CreateGetUserSetup()
                .Throws(new Exception("FakeException"));

            return users;
        }

        #endregion

        #region GetUserByEmail

        private static ISetup<IUsers, SearchUserResponse> CreateGetUserByEmailSetup(this Mock<IUsers> users)
        {
            return users.Setup(x => x.SearchUserByEmail(It.IsAny<string>(), It.IsAny<string>()));
        }

        public static Mock<IUsers> WithSearchUserByEmail(this Mock<IUsers> users, SearchUserResponse searchUserResponseToReturn)
        {
            users.CreateGetUserByEmailSetup()
                .Returns(searchUserResponseToReturn);

            return users;
        }

        public static Mock<IUsers> WithSearchUserByEmailThatReturnsUserFoundSearchUserResponse(this Mock<IUsers> users)
        {
            users.WithSearchUserByEmail(UsersApiResponseFactory.FakeSearchUserResponseUserFound());

            return users;
        }

        public static Mock<IUsers> WithSearchUserByEmailThatReturnsMultipleUsersFoundSearchUserResponse(this Mock<IUsers> users)
        {
            users.WithSearchUserByEmail(UsersApiResponseFactory.FakeSearchUserResponseUserMultipleUsersFound());

            return users;
        }

        public static Mock<IUsers> WithSearchUserByEmailThatReturnsUserNotFoundSearchUserResponse(this Mock<IUsers> users)
        {
            users.WithSearchUserByEmail(UsersApiResponseFactory.FakeSearchUserResponseUserNotFound());

            return users;
        }

        public static Mock<IUsers> WithSearchUserByEmailThatThrowsException(this Mock<IUsers> users)
        {
            users.CreateGetUserByEmailSetup()
                .Throws(new Exception("FakeException"));

            return users;
        }

        #endregion

        public static ISetup<IUsers, UpdateUserResponse> UpdateUserSetup(this Mock<IUsers> users)
        {
            return users.Setup(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<string>()));
        }

        public static Mock<IUsers> WithUpdateUser(this Mock<IUsers> users, UpdateUserResponse updateUserResponseToReturn)
        {
            users.UpdateUserSetup()
                .Returns(updateUserResponseToReturn);

            return users;
        }
    }
}
