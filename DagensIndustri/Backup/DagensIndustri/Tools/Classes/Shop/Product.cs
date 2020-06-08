using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using EPiServer.Core;
using DIClassLib.Misc;

namespace DagensIndustri.Tools.Classes.Shop
{
    [Serializable]
    public class Product
    {
        #region Properties
        public string Heading { get; set; }
        public string Subtitle { get; set; }
        public string ProductInformation1 { get; set; }
        public string ProductInformation2 { get; set; }
        public string PriceWithCurrency { get; set; }
        public double? Price { get; set; }
        public string Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDescription { get; set; }
        public string MainIntro { get; set; }
        public string MainBody { get; set; }
        public string ProductNumber { get; set; }
        public string ProductCategory { get; set; }
        public string ProductPageUrl { get; set; }
        public string TopImageUrl { get; set; }

        private List<string> additionalInformation;
        public List<string> AdditionalInformation
        {
            get
            {
                if (additionalInformation == null)
                    additionalInformation = new List<string>();
                return additionalInformation;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create a shop product with product information retrieved from a certain page
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public static Product CreateShopProduct(PageData pd)
        {
            Product product = new Product();
            if (pd != null)
            {                
                product.Heading = pd["Heading"] as string;
                product.Subtitle = pd["Subtitle"] as string;
                product.ProductInformation1 = pd["ProductInformation1"] as string;
                product.ProductInformation2 = pd["ProductInformation2"] as string;
                product.PriceWithCurrency = string.Format("{0} {1}", pd["Price"] as string, LanguageManager.Instance.Translate("/common/swedishcurrency"));
                
                double tempPrice;
                if (double.TryParse(pd["Price"] as string, out tempPrice))
                    product.Price = tempPrice;

                product.Quantity = pd["Quantity"] as string;
                product.ImageUrl = pd["Image"] as string;
                product.ImageDescription = pd["ImageDescription"] as string;
                product.MainIntro = pd["MainIntro"] as string;
                product.MainBody = pd["MainBody"] as string;
                product.ProductCategory = pd["ProductCategory"] as string;
                product.ProductPageUrl = EPiFunctions.GetFriendlyAbsoluteUrl(pd);
                product.TopImageUrl = pd["TopImage"] as string;
            }

            return product;
        }

        /// <summary>
        /// Create a list of shop products from the Adlibris xml
        /// </summary>
        /// <returns></returns>
        public static List<Product> CreateAdlibrisShopProducts()
        {
            List<Product> productList = new List<Product>();
            string url = MiscFunctions.GetAppsettingsValue("AdlibrisUrl");
            if (!string.IsNullOrEmpty(url))
            {
                XmlTextReader xmlReader = new XmlTextReader(url);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);

                foreach (XmlNode xmlNode in xmlDoc.SelectNodes("news_list/article"))
                {
                    productList.Add(CreateAdlibrisShopProduct(xmlNode));
                }
            }

            return productList;
        }

        /// <summary>
        /// Create an a shop product with product information retrieved from Adlibris xml
        /// </summary>
        /// <returns></returns>
        public static Product CreateAdlibrisShopProduct(string isbn)
        {
            Product product = new Product();

            string url = MiscFunctions.GetAppsettingsValue("AdlibrisUrl");
            if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(isbn))
            {
                XmlTextReader xmlReader = new XmlTextReader(url);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);

                string xpath = string.Format("/news_list/article[@isbn='{0}']", isbn);
                product = CreateAdlibrisShopProduct(xmlDoc.SelectSingleNode(xpath));
            }

            return product;
        }

        /// <summary>
        /// Get an adlibris product at a certain position
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Product CreateAdlibrisShopProduct(int index)
        {
            Product product = null;
            List<Product> productList = CreateAdlibrisShopProducts();
            if (productList != null && index > 0 && productList.Count >= index)
            {
                product = productList[index - 1];
            }
            return product;
        }

        /// <summary>
        /// Create a shop product from a certain Adlibris product
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private static Product CreateAdlibrisShopProduct(XmlNode xmlNode)
        {
            Product product = new Product();
            if (xmlNode != null)
            {
                product.Heading = MiscFunctions.GetXmlNodeText(xmlNode, "title");
                product.Subtitle = string.Format("{0}: {1}", LanguageManager.Instance.Translate("/common/by"), MiscFunctions.GetXmlNodeText(xmlNode, "author"));

                string price = EPiFunctions.GetAdlibrisPrice(MiscFunctions.GetXmlNodeText(xmlNode, "price"));
                product.PriceWithCurrency = string.Format("{0} {1}", price, LanguageManager.Instance.Translate("/common/swedishcurrency"));

                double tempPrice;
                if (double.TryParse(price, out tempPrice))
                    product.Price = tempPrice;

                product.Quantity = LanguageManager.Instance.Translate("/shop/quantity");
                product.ImageUrl = MiscFunctions.GetXmlNodeText(xmlNode, "product_image");
                product.ImageDescription = MiscFunctions.GetXmlNodeText(xmlNode, "title");
                product.MainIntro = MiscFunctions.GetXmlNodeText(xmlNode, "short_description");
                product.MainBody = MiscFunctions.GetXmlNodeText(xmlNode, "description");
                product.ProductNumber = MiscFunctions.GetXmlAttributeText(xmlNode, "isbn");
                product.ProductCategory = (string)EPiFunctions.GetPagePropertyValueBySettingsPage(null, "AdlibrisProductTemplate", "ProductCategory");
                product.ProductPageUrl = GetAdlibrisProductUrl(product.ProductNumber);
                product.TopImageUrl = product.ImageUrl;

                if (!string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(xmlNode, "publisher")))
                    product.AdditionalInformation.Add(string.Format("<b>{0}:</b> {1}", LanguageManager.Instance.Translate("/shop/publisher"), MiscFunctions.GetXmlNodeText(xmlNode, "publisher")));

                if (!string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(xmlNode, "format")))
                    product.AdditionalInformation.Add(string.Format("<b>{0}:</b> {1}", LanguageManager.Instance.Translate("/shop/format"), MiscFunctions.GetXmlNodeText(xmlNode, "format")));

                if (!string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(xmlNode, "language")))
                    product.AdditionalInformation.Add(string.Format("<b>{0}:</b> {1}", LanguageManager.Instance.Translate("/shop/language"), MiscFunctions.GetXmlNodeText(xmlNode, "language")));

                if (!string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(xmlNode, "date_published")))
                    product.AdditionalInformation.Add(string.Format("<b>{0}:</b> {1}", LanguageManager.Instance.Translate("/shop/datepublished"), MiscFunctions.GetXmlNodeText(xmlNode, "date_published")));

                if (!string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(xmlNode, "isbn")))
                    product.AdditionalInformation.Add(string.Format("<b>{0}:</b> {1}", LanguageManager.Instance.Translate("/shop/isbn"), MiscFunctions.GetXmlAttributeText(xmlNode, "isbn")));
            }

            return product;
        }

        /// <summary>
        /// Get url to an adlibris template product page
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private static string GetAdlibrisProductUrl(string productNumber)
        {
            string url = "#";
            PageReference productTemplatePageLink = EPiFunctions.SettingsPageSetting(null, "AdlibrisProductTemplate") as PageReference;
            if (productTemplatePageLink != null)
            {
                string productPageUrl = EPiFunctions.GetFriendlyAbsoluteUrl(productTemplatePageLink);
                url = string.Format("{0}?isbn={1}", productPageUrl, productNumber);
            }

            return url;
        }
        #endregion
    }
}