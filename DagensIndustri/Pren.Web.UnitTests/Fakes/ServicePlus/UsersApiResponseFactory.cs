using System.Collections.Generic;
using Di.ServicePlus.RestApi.ResponseModels.User;

namespace Pren.Web.UnitTests.Fakes.ServicePlus
{
    static class UsersApiResponseFactory
    {
        #region UserResponse
        public static UserResponse FakeUserResponseValidUser()
        {
            return new UserResponse
            {
                HttpResponseCode = "200",
                RequestId = "3lqOiMePevyyOWmKWX4Qus",
                User = new User
                {
                    AccountId = "226u8YytyzfA8i0S9ZUngu",
                    BrandId = "5DuzcZz0j8u0zArSNzZgHO",
                    Created = "1445413345458",
                    Email = "fioeufbweib@oegujb.se",
                    FirstName = "Kristoffergwreger",
                    LastName = "Janssongerge"
                }
            };
        }

        public static UserResponse FakeUserResponseValidUserWithBizSubscription()
        {
            var userResponse = FakeUserResponseValidUser();

            userResponse.User.ExternalUserIds = new List<ExternalUserIds>
            {
                new ExternalUserIds
                {
                    Id = "4037890",
                    System = "KAYAKBiz"
                }
            };

            return userResponse;
        }

        public static UserResponse FakeUserResponseExpiredToken()
        {
            return new UserResponse
            {
                HttpResponseCode = "401",
                ErrorCode = "TOKEN_EXPIRED",
                RequestId = "4tFRhcYcGSZsRJGzJ3CVSM"
            };
        }
        #endregion

        #region SearchUserResponse
        public static SearchUserResponse FakeSearchUserResponseUserFound()
        {
            return new SearchUserResponse
            {
                HttpResponseCode = "200",
                RequestId = "7u3uopzxgiEZvSWY4V6DwA",
                NumItems = "1",
                StartIndex = "0",
                TotalItems = "1",
                Query = "email_s:kristoffer.jansson@di.se",
                Users = new List<User>
                {
                    FakeUserResponseValidUser().User
                }
            };
        }

        public static SearchUserResponse FakeSearchUserResponseUserMultipleUsersFound()
        {
            var searchUsersResponse = FakeSearchUserResponseUserFound();
            searchUsersResponse.Users.Add(new User
            {
                AccountId = "bfouwebfojwenfoweufboweb",
                BrandId = "5DuzcZz0j8u0zArSNzZgHO",
                Created = "1445413345458",
                Email = "seconduser@fake.email",
                FirstName = "SecondUserFirstName",
                LastName = "SecondUserLastName"
            });

            return searchUsersResponse;
        }

        public static SearchUserResponse FakeSearchUserResponseUserNotFound()
        {
            return new SearchUserResponse
            {
                HttpResponseCode = "404",
                RequestId = "7u3uopzxgiEZvSWY4V6DwA",
                ErrorCode = "NOT_FOUND"
            };
        }
        #endregion

        #region CreateUser

        public static CreateUserResponse FakeCreateUserResponseSuccessfull()
        {
            return new CreateUserResponse
            {
                HttpResponseCode = "201",
                RequestId = "1sOZUFSLKmxsbyYnlUrX0Y",
                CreatedUser = new CreatedUser
                {
                    AccountId = "4J3YD7DgHsL6a514ScCuJB",
                    BrandId = "5DuzcZz0j8u0zArSNzZgHO",
                    Created = "1445353019477",
                    Email = "16okt_med12@jev-eltjanst.se",
                    Id = "2y06JdpqirinGNCPTczXhp",
                    Location = "/2y06JdpqirinGNCPTczXhp",
                    NextReminderTime = "1445439419450",
                    RemainingRemindersNumber = "2",
                    Type = "CUSTOMER",
                    Updated = "1445353019477"
                }
            };
        }

        #endregion

        #region UpdateUser

        public static UpdateUserResponse FakeUpdateUserResponseValidUser(User updatedUser)
        {
            return new UpdateUserResponse
            {
                HttpResponseCode = "200",
                RequestId = "3lqOiMePevyyOWmKWX4Qus",
                UpdatedUser = updatedUser
            };
        }

        #endregion
    }
}
