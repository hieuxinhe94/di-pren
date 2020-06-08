using Di.ServicePlus.RedirectApi;
using Di.ServicePlus.RedirectApi.Orders;
using StructureMap;

namespace Di.ServicePlus.Initialization
{
    public class DependencyResolver
    {
        public static ConfigurationExpression ConfigureStructureMapDependecies(ConfigurationExpression container)
        {
            container.For<IRedirectHandler>().Use<RedirectHandler>();

            container.For<IOrders>().Use<Orders>();

            return container;
        }
    }
}
