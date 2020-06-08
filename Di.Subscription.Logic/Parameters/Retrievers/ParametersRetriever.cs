using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.Parameters;
using Di.Subscription.Logic.Parameters.Types;

namespace Di.Subscription.Logic.Parameters.Retrievers
{
    public class ParametersRetriever : ParametersRetrieverBase, IParametersRetriever
    {        
        private readonly IParametersRepository _parametersRepository;

        public ParametersRetriever(IParametersRepository parametersRepository)
        {
            _parametersRepository = parametersRepository;
        }

        public IEnumerable<ReceiveType> GetAllReceiveTypes()
        {
            return GetAll(GetReceiveTypes);
        }

        public IEnumerable<TargetGroup> GetAllTargetGroups()
        {
            return GetAll(GetTargetGroups);
        }

        public IEnumerable<TargetGroup> GetTargetGroups(string paperCode)
        {
            var parameters = _parametersRepository.GetParameterValuesByGroup(paperCode, SubscriptionConstants.CodeGroupTargetGroups);

            var targetGroups = parameters.Where(parameter => parameter.CodeValue != string.Empty).Select(GetTargetGroup);

            // Remove duplicates
            return targetGroups.GroupBy(parameter => parameter.TargetGroupName).Select(group => group.First());              
        }

        public IEnumerable<ReceiveType> GetReceiveTypes(string paperCode)
        {
            var parameters = _parametersRepository.GetParameterValuesByGroup(paperCode, SubscriptionConstants.CodeGroupReceiveTypes);

            var receiveTypes = parameters.Where(parameter => parameter.CodeValue != string.Empty).Select(GetReceiveType);

            // Remove duplicates
            return receiveTypes.GroupBy(parameter => parameter.Name).Select(group => group.First());
        }

        private TargetGroup GetTargetGroup(Parameter parameter)
        {
            return new TargetGroup{ TargetGroupName = parameter.CodeValue };
        }

        private ReceiveType GetReceiveType(Parameter parameter)
        {
            return new ReceiveType { Name = parameter.CodeValue, Id = parameter.CodeNumber};
        }
    }
}
