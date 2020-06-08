using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Di.Common.Logging;
using Pren.Web.Business.Configuration;


namespace Pren.Web.Business.Mail
{
    public class NeolaneMailTrigger : IExternalMailTrigger
    {
        private readonly ILogger _logger;
        private readonly string _neolaneEndpoint;
        private readonly string _userName;
        private readonly string _password;

        public NeolaneMailTrigger(ILogger logger, ISiteSettings siteSettings)
        {
            _logger = logger;
            _neolaneEndpoint = siteSettings.NeoLaneEndPointUrl;
            _userName = siteSettings.NeoLaneEndPointUser;
            _password = siteSettings.NeoLaneEndPointPassword;
        }

        public async Task<TriggerExternalMailResult> InvokeExternalMailAsync(Dictionary<string, string> parameters, string workflowId)
        {
            try
            {
                var token = await GetNeolaneTokenAsync(_userName, _password);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("SOAPAction", "xtk:workflow#PostEvent");
                    client.DefaultRequestHeaders.Add("X-Security-Token", token.SecurityToken);
                    client.DefaultRequestHeaders.Add("cookie", "__sessiontoken=" + token.SessionToken);

                    var content = new StringContent(CreateTriggerRequestString(token, workflowId, parameters), Encoding.UTF8, "text/xml");

                    using (var response = await client.PostAsync(_neolaneEndpoint, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return new TriggerExternalMailResult
                            {
                                Success = response.IsSuccessStatusCode
                            };
                        }

                        _logger.Log("Error - Neolane mail trigger response: " + response.StatusCode, LogLevel.Error, typeof(NeolaneMailTrigger));
                        return new TriggerExternalMailResult
                        {
                            Success = false
                        };
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "Error - Neolane mail trigger", LogLevel.Error, typeof(NeolaneMailTrigger));
                return new TriggerExternalMailResult
                {
                    Success = false
                };
            }
        }

        private async Task<NeolaneToken> GetNeolaneTokenAsync(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("SOAPAction", "xtk:session#Logon");
                var content = new StringContent(CreateSessionRequestString(userName, password), Encoding.UTF8, "text/xml");

                using (var response = await client.PostAsync(_neolaneEndpoint, content))
                {
                    var soapResponse = await response.Content.ReadAsStringAsync();
                    return ParseSessionSoapRequest(soapResponse);                    
                }
            }
        }

        private NeolaneToken ParseSessionSoapRequest(string response)
        {
            var soap = XDocument.Parse(response);
            XNamespace ns = "urn:xtk:session";

            var tokenResponse = new NeolaneToken
            {
                SecurityToken = soap.Descendants(ns + "pstrSecurityToken").First().Value,
                SessionToken = soap.Descendants(ns + "pstrSessionToken").First().Value
            };

            return tokenResponse;
        }

        private TriggerExternalMailResult ParseTriggerSoapResponse(string response)
        {
            var soap = XDocument.Parse(response);       
            XNamespace ns = "urn:xtk:session";
            
            var result = soap.Descendants(ns + "pstrSessionToken").First().Value;

            return new TriggerExternalMailResult();
        }

        private string CreateSessionRequestString(string userName, string password)
        {
            return $@"
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:xtk:session"">
                    <soapenv:Header/>
                    <soapenv:Body>
                        <urn:Logon>
                            <urn:sessiontoken/>
                            <urn:strLogin>{userName}</urn:strLogin>     
                            <urn:strPassword>{password}</urn:strPassword>          
                            <urn:elemParameters />
                        </urn:Logon>
                    </soapenv:Body>
                </soapenv:Envelope>";
        }

        private string CreateTriggerRequestString(NeolaneToken token, string workflowId, Dictionary<string, string> parameters)
        {
            var variablesElement = CreateVariablesElement(parameters);

            return $@"
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:xtk:workflow"">
                    <soapenv:Header/>
                    <soapenv:Body>
                        <urn:PostEvent>
                            <urn:sessiontoken>{token.SessionToken}</urn:sessiontoken>
                            <urn:strWorkflowId>{workflowId}</urn:strWorkflowId>
                            <urn:strActivity>signal</urn:strActivity>
                            <urn:strTransition></urn:strTransition>
                            <urn:elemParameters>
                                {variablesElement}
                            </urn:elemParameters>
                            <urn:bComplete>0</urn:bComplete>
                        </urn:PostEvent>
                    </soapenv:Body>
                </soapenv:Envelope>";
        }

        private string CreateVariablesElement(Dictionary<string, string> parameters)
        {
            var variables = new StringBuilder();
            variables.Append("<variables ");
            foreach (var parameter in parameters)
            {
                variables.Append($@"{parameter.Key}=""{parameter.Value}"" ");
            }
            variables.Append("/>");

            return variables.ToString();
        }
    }

    internal class NeolaneToken
    {
        public string SessionToken { get; set; }
        public string SecurityToken { get; set; }
    }
}