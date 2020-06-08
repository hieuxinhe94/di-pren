using System.Web;

namespace Di.ServicePlus.RedirectApi.Orders
{
    public interface IOrders
    {
        string GetUrl(string appId, string productId, string secretKey, string email, string firstName,
            string lastName, string careOf, string phone, string streetAddress, string streetNumber, string zip, string city, string companyName, string personalNumber,
            string payerIdentificationNumber, string payerName, string payerAttention, string payerContactnumber, string payerStreetName, string payerStreetNumber, string payerZipCode, string payerCity,
            string payMethod, string targetGroup, string campNo, string priceList, string price,
            string productDescription, string callBackUrl, bool isPaperProduct, string subsKind, string salesChannel);

        OrderResponse GetResponse(HttpRequestBase request);
    }
}