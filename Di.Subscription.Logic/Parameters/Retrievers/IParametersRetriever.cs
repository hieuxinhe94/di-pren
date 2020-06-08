using System.Collections.Generic;
using Di.Subscription.Logic.Parameters.Types;

namespace Di.Subscription.Logic.Parameters.Retrievers
{
    public interface IParametersRetriever
    {
        IEnumerable<ReceiveType> GetAllReceiveTypes();
        IEnumerable<ReceiveType> GetReceiveTypes(string paperCode);
        IEnumerable<TargetGroup> GetAllTargetGroups();
        IEnumerable<TargetGroup> GetTargetGroups(string paperCode);
    }
}