define([
    "jquery",
    "pubsub",
    "json!/api/mysettings/profile/getprofile"],
    function ($, pubsub, subscriberJson) {
        return new Subscriber(subscriberJson);
    });

function Subscriber(subscriberJson) {
    this.setProperties(subscriberJson); 
}

Subscriber.prototype.setProperties = function (profile) {

    function getJsonValue(value) {
        return value != null ? value : "";
    }

    if (profile.CustomerNumber == 0) return;    

    this.customerNumber = getJsonValue(profile.CustomerNumber);
    this.firstName = getJsonValue(profile.FirstName);
    this.lastName = getJsonValue(profile.LastName);
    this.phone = getJsonValue(profile.Phone);
    this.email = getJsonValue(profile.Email);
    this.streetName = getJsonValue(profile.AddressStreetName);
    this.streetNumber = getJsonValue(profile.AddressStreetNumber);
    this.zip = getJsonValue(profile.AddressZip);
    this.city = getJsonValue(profile.AddressCity);
    this.co = getJsonValue(profile.AddressCareOf);
    this.stairs = getJsonValue(profile.AddressStairs);
    this.stairCase = getJsonValue(profile.AddressStairCase);
    this.closestIssueDate = profile.ClosestIssueDate.substring(0, 10);
    this.maxEndDate = profile.MaxEndDate.substring(0, 10);
    this.activeSubscriptions = getActiveSubscriptions(profile.ActiveSubscriptions);
    this.myCodesSettings = {
        userId: profile.MyCodesSettings.UserId,
        token: profile.MyCodesSettings.Token
    }

    function getActiveSubscriptions(subscriptionsJson) {

        var activeSubscriptions = [];

        $.each(subscriptionsJson, function (key, subscription) {
            activeSubscriptions.push(
                new SubscriptionItem(
                subscription.SubscriptionNumber,
                subscription.IsDigitalSubscription,
                subscription.IsActive,
                subscription.StartDate,
                subscription.EndDate,
                subscription.ProductName,
                subscription.ClosestIssueDate,
                subscription.GenerationNumber
            ));
        });

        return activeSubscriptions;
    }
}

Subscriber.prototype.getFullAddress = function () {
    var address = this.streetName + ' ' + this.streetNumber;
    if (this.stairs != null) {
        address += ' ' + this.stairs;
    }
    if (this.co != null) {
        address += ' CO: ' + this.co;
    }
    address += ' ' + this.zip;
    address += ' ' + this.city;

    return address;
}

Subscriber.prototype.setSelectedSubscription = function (subscriptionNo, generationNo) {
    var self = this;
   
    var subscriptionArray = $.grep(self.activeSubscriptions, function (subscription) {
        return subscription.subscriptionNumber == subscriptionNo && subscription.generationNumber == generationNo;
    });

    this.selectedSubscription = subscriptionArray[0];
    $.publish('subscriptionChanged');
}

Subscriber.prototype.changeProperty = function(name, value) {
    this[name] = value;

    $.publish('subscriberChanged');
}

function SubscriptionItem(subscriptionNumber, isDigital, isActive, startDate, endDate, productName, closestIssueDate, generationNumber) {
    this.subscriptionNumber = subscriptionNumber;
    this.isDigital = isDigital;
    this.isActive = isActive;
    this.startDate = startDate.substring(0, 10);
    this.endDate = endDate.substring(0, 10);
    this.productName = productName;
    this.closestIssueDate = closestIssueDate.substring(0, 10);
    this.generationNumber = generationNumber;
}