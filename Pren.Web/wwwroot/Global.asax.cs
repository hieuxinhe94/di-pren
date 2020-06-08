using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using EPiServer.Web.Routing;
using System.Net;

namespace Pren.Web
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // To prevent "The request was aborted: Could not create SSL/TLS secure channel" against S+ (confirmed in QA)
            // https://stackoverflow.com/questions/28286086/default-securityprotocol-in-net-4-5
            // https://stackoverflow.com/questions/2859790/the-request-was-aborted-could-not-create-ssl-tls-secure-channel
            // Thorben 180601

            // Turn on TLS 1.1 and 1.2
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Turn off SSL3
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
        }

        protected override void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            AddCodePortalApiRoutes(routes);
            AddBizSubscriberApiRoutes(routes);
            AddMySettingsApiRoutes(routes);
            AddContentApiRoutes(routes);

            routes.MapHttpRoute("CampaignApi", "api/campaign/{updateCache}",
                new
                {
                    controller = "Campaign",
                    action = "Get",
                    updateCache = RouteParameter.Optional
                });

            routes.MapHttpRoute("EmailCheckApi", "api/emailcheck",
                new
                {
                    controller = "EmailCheck",
                    action = "Get"
                });

            routes.MapHttpRoute("EmailExistsApi", "api/emailexists",
                new
                {
                    controller = "EmailCheck",
                    action = "GetEmailMessage"
                });

            routes.MapHttpRoute("FaqApiItems", "api/faq/getitems/{limit}/{sortorder}",
                new
                {
                    controller = "Faq",
                    action = "GetItems"
                });


            routes.MapHttpRoute("FaqApiTopics", "api/faq/GetTopics/{limit}/{sortorder}",
                new
                {
                    controller = "Faq",
                    action = "GetTopics"
                });

            routes.MapHttpRoute("FaqApiItemsByTopic", "api/faq/GetItemsByTopics/{topic}/{limit}/{sortorder}",
                new
                {
                    controller = "Faq",
                    action = "GetItemsByTopics"
                });

            routes.MapHttpRoute("AddressApi", "api/address/{input}",
                new
                {
                    controller = "Address",
                    action = "Get",
                });

            routes.MapHttpRoute("MaskedAddressApi", "api/maskedaddress/{input}",
                new
                {
                    controller = "MaskedAddress",
                    action = "Get",
                });

            routes.MapHttpRoute("StartDateApi", "api/startdate/{campid}",
                new
                {
                    controller = "StartDate",
                    action = "Get",
                });

            routes.MapHttpRoute("StudentCheckApi", "api/studentcheck/{socialSecurityNumber}",
                new
                {
                    controller = "StudentCheck",
                    action = "Get",
                });

            routes.MapHttpRoute("PostNameApi", "api/postname/{zipcode}",
                new
                {
                    controller = "PostName",
                    action = "Get",
                });

            routes.MapRoute("GetTempAddress", "TemporaryAddressChangePage/GetTempAddress/{addressId}",
                new
                {
                    controller = "TemporaryAddressChangePage",
                    action = "GetTempAddress",
                });

            routes.MapContentRoute("ShowInvoice", "{language}/{node}/pdf/{customerNumber}/{invoiceGuid}/{action}",
            new
            {
                controller = "MyStartPageController",
                action = "ShowInvoice",
            });

            routes.MapContentRoute("CampaignOrder", "{language}/{node}/order/{packageId}/{action}",
            new
            {
                controller = "OrderFlowCampaignPageController",
                action = "Order",
            });


            base.RegisterRoutes(routes);
        }

        private void AddMySettingsApiRoutes(RouteCollection routes)
        {
            var routeList = new List<Route>
            {
                routes.MapHttpRoute("ProfileApi", "api/mysettings/profile/getprofile",
                    new
                    {
                        controller = "Profile",
                        action = "GetProfile"
                    }),
                routes.MapHttpRoute("ProfileNameApi", "api/mysettings/profile/updatename/{firstName}/{lastName}",
                    new
                    {
                        controller = "Profile",
                        action = "UpdateName"
                    }),
                routes.MapHttpRoute("ProfileEmailApi", "api/mysettings/profile/updateemail/{email}",
                    new
                    {
                        controller = "Profile",
                        action = "UpdateEmail"
                    }),
                routes.MapHttpRoute("ProfilePhoneApi", "api/mysettings/profile/updatephone/{phone}",
                    new
                    {
                        controller = "Profile",
                        action = "UpdatePhone",
                        phone = RouteParameter.Optional
                    }),
                routes.MapHttpRoute("ProfileAddressApi", "api/mysettings/profile/addresschanges",
                    new
                    {
                        controller = "Profile",
                        action = "GetPermanentAddressChanges",
                    }),
                routes.MapHttpRoute("ProfileNewAddressApi", "api/mysettings/profile/savepermaddress",
                    new
                    {
                        controller = "Profile",
                        action = "SavePermanentAddress",
                    }),
                routes.MapHttpRoute("ProfileDeleteAddressApi", "api/mysettings/profile/deletepermaddress",
                    new
                    {
                        controller = "Profile",
                        action = "DeletePermanentAddress",
                    }),
                routes.MapHttpRoute("ProfileEditAddressApi", "api/mysettings/profile/editpermaddress",
                    new
                    {
                        controller = "Profile",
                        action = "GetEditPermanentAddress",
                    }),
                routes.MapHttpRoute("ProfileInvoicesApi", "api/mysettings/profile/getinvoices",
                    new
                    {
                        controller = "Profile",
                        action = "GetInvoices",
                    }),

                routes.MapHttpRoute("ProfileEventsApi", "api/mysettings/profile/events",
                    new
                    {
                        controller = "Profile",
                        action = "GetProfileEvents",
                    }),

                /***** SUBSCRIPTION *****/

                routes.MapHttpRoute("SubscriptionGetReclaimDetailsApi", "api/mysettings/subscription/getreclaimdetails/{subscriptionNumber}",
                    new
                    {
                        controller = "Subscription",
                        action = "GetReclaimDetails",
                    }),
                routes.MapHttpRoute("SubscriptionSaveReclaimApi", "api/mysettings/subscription/savereclaim",
                    new
                    {
                        controller = "Subscription",
                        action = "SaveReclaim",
                    }),
                routes.MapHttpRoute("SubscriptionGetSubsSleepApi", "api/mysettings/subscription/getsubssleeps/{subscriptionNumber}",
                    new
                    {
                        controller = "Subscription",
                        action = "GetSubscriptionSleeps",
                    }),
                routes.MapHttpRoute("SubscriptionSaveSubsSleepApi", "api/mysettings/subscription/savesubssleep",
                    new
                    {
                        controller = "Subscription",
                        action = "SaveSubscriptionSleep",
                    }),
                routes.MapHttpRoute("SubscriptionDeleteSubsSleepApi", "api/mysettings/subscription/deletesubssleep/{subscriptionNumber}/{startDate}/{endDate}",
                    new
                    {
                        controller = "Subscription",
                        action = "DeleteSubscriptionSleep",
                    }),
                routes.MapHttpRoute("SubscriptionChangeSubsSleepApi", "api/mysettings/subscription/changesubssleep",
                    new
                    {
                        controller = "Subscription",
                        action = "ChangeSubscriptionSleep",
                    }),

                routes.MapHttpRoute("SubscriptionGetTpaApi", "api/mysettings/subscription/tmpaddresschanges/{subscriptionNumber}",
                    new
                    {
                        controller = "Subscription",
                        action = "GetTemporaryAddressChanges",
                    }),
                routes.MapHttpRoute("SubscriptionDeleteTpaApi", "api/mysettings/subscription/deletetmpaddress",
                    new
                    {
                        controller = "Subscription",
                        action = "DeleteTemporaryAddressChanges",
                    }),
                routes.MapHttpRoute("SubscriptionSaveTpaApi", "api/mysettings/subscription/savetmpaddress",
                    new
                    {
                        controller = "Subscription",
                        action = "SaveTemporaryAddressChanges",
                    }),
                routes.MapHttpRoute("SubscriptionChangeTpaApi", "api/mysettings/subscription/changetmpaddress",
                    new
                    {
                        controller = "Subscription",
                        action = "ChangeTemporaryAddressChange",
                    }),

                    /***** LOGGING *****/

                routes.MapHttpRoute("LoggingApi", "api/mysettings/logging/log",
                    new
                    {
                        controller = "Logging",
                        action = "Log",
                    }),

                    /***** CONTACT *****/
                routes.MapHttpRoute("ContactCustomerServiceApi", "api/mysettings/contactcustomerservice/contact",
                    new
                    {
                        controller = "ContactCustomerService",
                        action = "Contact"
                    }),

                routes.MapHttpRoute("CancelSubscriptionServiceApi", "api/mysettings/contactcustomerservice/cancel",
                    new
                    {
                        controller = "ContactCustomerService",
                        action = "Cancel"
                    }),

                /***** Codeportal which needs session *****/
                routes.MapHttpRoute("CreateNewGiveAwayApi", "api/codeportal/createnewgiveaway/{token}/{listId}/{giveToEmail}",
                    new
                    {
                        controller = "CodePortalCode",
                        action = "CreateNewGiveAway"
                    })
            };

            foreach (var route in routeList)
            {
                route.RouteHandler = new HttpControllerRouteHandlerSessionState();    
            }
        }

        private void AddCodePortalApiRoutes(RouteCollection routes)
        {

            var routeList = new List<Route>
            {
                routes.MapHttpRoute("GetExistingCodeApi", "api/codeportal/getexistingcode/{userId}/{listId}",
                    new
                    {
                        controller = "CodePortalCode",
                        action = "GetExistingCode",
                    }),

                routes.MapHttpRoute("GetNewCodeApi", "api/codeportal/getnewcode/{token}/{listId}",
                    new
                    {
                        controller = "CodePortalCode",
                        action = "GetNewCode",
                    }),

                routes.MapHttpRoute("GetExistingGiveAwayApi", "api/codeportal/getexistinggiveaway/{userId}/{listId}",
                    new
                    {
                        controller = "CodePortalCode",
                        action = "GetExistingGiveAway"
                    })
            };

            foreach (var route in routeList)
            {
                route.RouteHandler = new HttpControllerRouteHandlerSessionState();
            }

        }

        private void AddBizSubscriberApiRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute("MarkActiveBizSubscriberForRemovalApi", "api/biz/markactivebizsubscriberforremoval/{bizSubscriptionId}/{userId}/{markForRemoval}",
                new
                {
                    controller = "MarkActiveBizSubscriberForRemoval",
                    action = "MarkForRemoval",
                });

            routes.MapHttpRoute("InviteBizSubscriberApi", "api/biz/invitebizsubscriber/{bizSubscriptionId}/{email}",
                new
                {
                    controller = "InviteBizSubscriber",
                    action = "Invite",
                });

            routes.MapHttpRoute("RemindBizSubscriberApi", "api/biz/remindbizsubscriber/{bizSubscriptionId}/{code}",
                new
                {
                    controller = "InviteBizSubscriber",
                    action = "Remind",
                });

            routes.MapHttpRoute("DeletePendingBizSubscriberApi", "api/biz/deletependingbizsubscriber/{bizSubscriptionId}/{code}",
                new
                {
                    controller = "DeletePendingBizSubscriber",
                    action = "DeletePending",
                });

            routes.MapHttpRoute("GetActiveBizSubscribersApi", "api/biz/getactivebizsubscribers/{bizSubscriptionId}/{skip}/{take}",
                new
                {
                    controller = "GetActiveBizSubscribers",
                    action = "GetSubscribers",
                    skip = "0",
                    take = "-1"                   
                });

            routes.MapHttpRoute("GetPendingBizSubscribersApi", "api/biz/getpendingbizsubscribers/{bizSubscriptionId}/{skip}/{take}",
                new
                {
                    controller = "GetPendingBizSubscribers",
                    action = "GetSubscribers",
                    skip = "0",
                    take = "-1"  
                });

            routes.MapHttpRoute("CheckBizSubscriptionApi", "api/biz/checkbizsubscription/{userId}",
                new
                {
                    controller = "CheckBizSubscription",
                    action = "Get",
                });

            routes.MapHttpRoute("GetBizSubscriptionCountApi", "api/biz/getbizsubscriptioncount/{bizSubscriptionId}",
                new
                {
                    controller = "GetBizSubscriptionCount",
                    action = "GetCount",
                });

            routes.MapHttpRoute("GetBizSubscriberAddressInfoApi", "api/biz/getbizsubscriberaddressinfo/{customerNumber}",
                new
                {
                    controller = "GetBizSubscriberAddressInfo",
                    action = "Get",
                });
        }

        private void AddContentApiRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute("GetAlertsApi", "api/content/{brand}/alerts",
                new
                {
                    controller = "Alert",
                    action = "GetAlerts"
                });

            routes.MapHttpRoute("GetTeasersApi", "api/content/{brand}/teasers",
                new
                {
                    controller = "Teaser",
                    action = "GetTeasers"
                });

            routes.MapHttpRoute("GetRightColumnTeasersApi", "api/content/{brand}/rightcolumnteasers",
                new
                {
                    controller = "RightColumnTeaser",
                    action = "GetRightColumnTeasers"
                });

            routes.MapHttpRoute("GetCodesApi", "api/content/{brand}/codes",
                new
                {
                    controller = "Code",
                    action = "GetCodes"
                });


            routes.MapHttpRoute("GetTermsApi", "api/content/{brand}/terms/{type}",
                new
                {
                    controller = "Text",
                    action = "GetText"
                });

            routes.MapHttpRoute("GetTextsApi", "api/content/{brand}/texts/{type}",
                new
                {
                    controller = "Text",
                    action = "GetText"
                });

            routes.MapHttpRoute("GetCompanyMessages", "api/content/{brand}/companyservice/messages",
                new
                {
                    controller = "CompanyServiceContentApi",
                    action = "GetMessages"
                });
        }
    }
    
    public class MyHttpControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public MyHttpControllerHandler(RouteData routeData) : base(routeData)
        {
            
        }
    }
    public class HttpControllerRouteHandlerSessionState : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MyHttpControllerHandler(requestContext.RouteData);
        }
    }
}