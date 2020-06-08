using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DIClassLib.OneByOne;
using System.Text.RegularExpressions;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using System.Data;
using DIClassLib.DbHelpers;

namespace DagensIndustri.Templates.Public.Pages.C_Campaign
{
    public class CampaignHelper
    {
        public static string SaveEmail(string email) {

            return "ok";
        }

        public static string GetCity(string zip) 
        {
            return MsSqlHandler.MdbGetPostalNameByZip(zip);
        }

        public static ParInfoResult GetNormalizedParData(string input, string campaign, string pageId)
        {
            var result = new ParInfoResult();

            //9697608330 KJTH, Apelgatan 5 A
            //5560163429 SSAB, Klarabergsviadukten 70 Uppg D
            //5563687184 Swedia, SKINNARVIKSRINGEN 16 LGH 2
            //5567762686 Supplement company, ELISETORPSVÄGEN 15 C LGH 1602
            //5564128980 Bjersbo, Flintlåsvägen 18 3tr Lgh1302

            if (new Regex(@"(^[\d]{10,12}$)").IsMatch(input))
            {
                input = input.Length == 10 ? "19" + input : input;
                var person = Obo.GetPerson(input);

                if (person != null && !string.IsNullOrEmpty(person.FirstNames)) 
                {
                    result.FirstNames = string.IsNullOrEmpty(person.GivenNames) ? person.FirstNames : person.GivenNames; ;
                    result.LastNames = person.LastNames;
                    result.GivenNames = person.GivenNames;
                    result.StreetAddressRaw = person.StreetAddressRaw;
                    result.ZipCode = person.ZipCode;
                    result.City = person.City;
                    result.PhoneMobile = person.PhoneMobile;
                    result.Name = string.Empty;
                    result.CompanyNumber = string.Empty;

                    MsSqlHandler.InsertToLogPar(int.Parse(pageId),
                        campaign,
                        input,
                        string.Empty,
                        result.FirstNames,
                        result.LastNames,
                        string.Empty,
                        result.StreetAddressRaw ?? string.Empty,
                        result.ZipCode,
                        "", //TxtCity.Text,
                        result.PhoneMobile);
                }
                else 
                {
                    result.Error = "Vi hittar inga personuppgifter för " + input;
                }
            }
            else if (new Regex(@"(^[\d]{6}(-)[\d]{4}$)").IsMatch(input))
            {
                var company = Obo.GetCompany(input.Replace("-",string.Empty));

                if (company != null && !string.IsNullOrEmpty(company.Name))
                {
                    result.FirstNames = string.Empty;
                    result.LastNames = string.Empty;
                    result.GivenNames = string.Empty;
                    result.PhoneMobile = string.Empty;
                    result.StreetAddressRaw = company.StreetAddressRaw;
                    result.ZipCode = company.ZipCode;
                    result.City = company.City;
                    result.Name = company.Name;
                    result.CompanyNumber = company.CompanyNumber;

                    MsSqlHandler.InsertToLogPar(int.Parse(pageId),
                        campaign,
                        string.Empty,
                        result.CompanyNumber,
                        string.Empty,
                        string.Empty,
                        result.Name,
                        result.StreetAddressRaw ?? string.Empty,
                        result.ZipCode,
                        "", //TxtCity.Text, 
                        string.Empty);
                }
                else
                {
                    result.Error = "Vi hittar inga företagsuppgifter för " + input;
                }
            }
            else 
            {
                result.Error = "Felaktigt format, ÅÅÅÅMMDDXXXX eller XXXXXXXXXX";
            }

            CirixifyAddressFields(result);

            return result;
        }

        /// <summary>
        /// Helper for fixing address from PAR to Cirix-format
        /// </summary>       
        private static void CirixifyAddressFields(ParInfoResult parInfoResult)
        {
            if (parInfoResult.StreetAddressRaw == null)
                return;

            string streetAddress = string.Empty, streetNo = string.Empty, stairCase = string.Empty, stairs = string.Empty, appNo = string.Empty;

            var nextIsAppNo = false;

            foreach (var arrayItem in parInfoResult.StreetAddressRaw.Split(' '))
            {
                if (nextIsAppNo)
                {
                    appNo = arrayItem;
                    nextIsAppNo = false;
                    continue;
                }

                if (arrayItem.ToLower().EndsWith("tr"))
                    stairs = arrayItem.ToLower().Replace("tr", string.Empty);
                else if (arrayItem.ToLower().Equals("lgh"))
                    nextIsAppNo = true;
                else if (arrayItem.ToLower().StartsWith("lgh"))
                    appNo = arrayItem.ToLower().Replace("lgh", string.Empty);
                else if (MiscFunctions.IsNumeric(arrayItem) && string.IsNullOrEmpty(streetNo))
                    streetNo = arrayItem;
                else if (arrayItem.Length == 1 && !MiscFunctions.IsNumeric(arrayItem) && string.IsNullOrEmpty(stairCase))
                    stairCase = arrayItem;
                else if (!MiscFunctions.IsNumeric(arrayItem)) //As long it isn't numeric, it must be street address
                    streetAddress += arrayItem + " ";
            }

            parInfoResult.StreetAddress = streetAddress;
            parInfoResult.HouseNumber = streetNo;
            parInfoResult.StairCase = stairCase;
            parInfoResult.Stairs = stairs;
            parInfoResult.AppartmentNumber = appNo;
        }


        public static string GetSubStartDate(int pageId, string campId)
        {
            try
            {
                var dsCamp = CirixDbHandler.GetCampaign(campId);
                var paperCode = dsCamp.Tables[0].Rows[0]["PAPERCODE"].ToString();
                var productNo = dsCamp.Tables[0].Rows[0]["PRODUCTNO"].ToString();
                return CirixDbHandler.GetNextIssueDateIncDiRules(DateTime.Now, paperCode, productNo).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                new Logger("Campaign.GetSubStartDate() failed on page with id: '" + pageId + "' failed", ex.ToString());
                return DateTime.Now.ToString("yyyy-MM-dd");
            }        
        }

        public class ParInfoResult 
        {

            public string FirstNames { get; set; }
            public string GivenNames { get; set; }
            public string LastNames { get; set; }

            public string StreetAddressRaw { get; set; }

            public string StreetAddress { get; set; }
            public string HouseNumber { get; set; }
            public string Stairs { get; set; }
            public string StairCase { get; set; }
            public string AppartmentNumber { get; set; }

            public string ZipCode { get; set; }
            public string City { get; set; }

            public string PhoneMobile { get; set; }

            public string Name { get; set; }
            public string CompanyNumber { get; set; }

            public string Error { get; set; }
                
        }
        
    }
}