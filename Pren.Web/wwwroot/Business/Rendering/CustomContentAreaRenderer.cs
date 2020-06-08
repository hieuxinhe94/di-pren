using System;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;

namespace Pren.Web.Business.Rendering
{
    public class CustomContentAreaRenderer : ContentAreaRenderer
    {
        protected override string GetContentAreaItemCssClass(HtmlHelper htmlHelper, ContentAreaItem contentAreaItem)
        {
            var tag = GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem);

            //return string.Format("block {0} {1} {2}", GetTypeSpecificCssClasses(contentAreaItem, ContentRepository), GetCssClassForTag(tag), tag);
            return string.Format("block {0} {1}", GetTypeSpecificCssClasses(contentAreaItem, ContentRepository), tag);
        }

        ///// <summary>
        ///// Gets a CSS class used for styling based on a tag name (ie a Bootstrap class name)
        ///// </summary>
        ///// <param name="tagName">Any tag name available, see <see cref="RenderingConstants.ContentAreaTags"/></param>
        //private static string GetCssClassForTag(string tagName)
        //{
        //    if (string.IsNullOrEmpty(tagName))
        //    {
        //        return "";
        //    }
        //    switch (tagName.ToLower())
        //    {
        //        case "col-md-12":
        //            return "full";
        //        case "col-md-8":
        //            return "wide";
        //        case "col-md-6":
        //            return "half";
        //        case "col-md-4":
        //            return "narrow";
        //        default:
        //            return string.Empty;
        //    }
        //}

        private static string GetTypeSpecificCssClasses(ContentAreaItem contentAreaItem, IContentRepository contentRepository)
        {
            var content = contentAreaItem.GetContent(contentRepository);
            var cssClass = content == null ? String.Empty : content.GetOriginalType().Name.ToLowerInvariant();

            var customClassContent = content as ICustomCssInContentArea;
            if (customClassContent != null && !string.IsNullOrWhiteSpace(customClassContent.ContentAreaCssClass))
            {
                cssClass += string.Format(" {0}", customClassContent.ContentAreaCssClass);
            }

            return cssClass;
        }
    }
}
