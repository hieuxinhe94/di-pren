using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Di.Common.Utils;
using Pren.Web.Business.Invoice.ExtendedModels;
using Pren.Web.Business.Invoice.ResponseModels;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Business.Invoice
{
    public interface IInvoiceApi
    {
        Task<IEnumerable<CustomerInvoice>> GetInvoicesAsync(long customerNumber);
        Task<byte[]> GetInvoiceByteArrayAsync(string customerNumber, string invoiceGuid);
    }

    class InvoiceApi : IInvoiceApi
    {
        private readonly string _apiUrl;
        private readonly string _apiStreamUrl;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _encryptKey;
        private readonly string _encryptIv;

        public InvoiceApi(string invoiceApiUrl, string invoiceApiStreamUrl, string userName, string password, string encryptKey, string encryptIv)
        {
            _apiUrl = invoiceApiUrl;
            _apiStreamUrl = invoiceApiStreamUrl;
            _userName = userName;
            _password = password;
            _encryptKey = encryptKey;
            _encryptIv = encryptIv;
        }


        public async Task<IEnumerable<CustomerInvoice>> GetInvoicesAsync(long customerNumber)
        {
            var url = string.Format(_apiUrl, "invoice_di", customerNumber);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = GetBasicAuthenticationHeader(_userName, _password);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                using (var response = await client.GetAsync(url))
                {
                    var responseJsonString = await response.Content.ReadAsStringAsync();

                    var invoices = responseJsonString.ConvertToObject<SfInvoiceResponse>();

                    return invoices.Query.Results.Select(t => new SfInvoice(t, _encryptKey, _encryptIv));
                }
            }
        }

        public async Task<byte[]> GetInvoiceByteArrayAsync(string customerNumber, string invoiceGuid)
        {
            var invoiceId = EncryptUtil.DecryptString(invoiceGuid, _encryptKey, _encryptIv);

            var url = string.Format(_apiStreamUrl, invoiceId);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = GetBasicAuthenticationHeader(_userName, _password);

                using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
        }

        private AuthenticationHeaderValue GetBasicAuthenticationHeader(string userName, string passWord)
        {
            var parameter = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + passWord));
            var authenticationHeader = new AuthenticationHeaderValue("Basic", parameter);
            return authenticationHeader;
        }
    }
}