using System.Collections.Generic;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.Parameters
{
    internal class ParametersRepository : IParametersRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public ParametersRepository(ISubscriptionDataAccess subscriptionDataAccess, IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<Parameter> GetParameterValuesByGroup(string paperCode, string codeGroupNumber)
        {
            var parameterValuesDataSet = _subscriptionDataAccess.GetParameterValuesByGroup(paperCode, codeGroupNumber);

            return _objectConverter.ConvertFromDataSet<Parameter>(parameterValuesDataSet);
        }
    }
}
