using System;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Core;

namespace Pren.Web.UnitTests.Fakes.Content
{
    public static class PageExtensions
    {
        public static TSource WithPagePropertyValue<TSource, TPropertyValue>(this TSource source, Expression<Func<TSource, TPropertyValue>> expression, TPropertyValue result) where TSource : IContent
        {
            var paramExp = expression.Parameters.Single();
            var assignExp = Expression.Assign(expression.Body, Expression.Constant(result));
            var lambdaExp = Expression.Lambda(assignExp, paramExp);
            lambdaExp.Compile().DynamicInvoke(source);

            return source;
        }
    }
}
