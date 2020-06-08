using System;
using System.Web.Mvc;
using System.Web.UI;

namespace Pren.Web.Business.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : OutputCacheAttribute
    {
        public NoCacheAttribute()
        {
            Duration = 0;
            Location = OutputCacheLocation.None;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var cache = filterContext.HttpContext.Response.Cache;
            cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));
        }
    }  
}