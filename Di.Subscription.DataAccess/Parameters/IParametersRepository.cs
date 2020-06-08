using System.Collections.Generic;

namespace Di.Subscription.DataAccess.Parameters
{
    public interface IParametersRepository
    {
        IEnumerable<Parameter> GetParameterValuesByGroup(string paperCode, string codeGroupNumber);
    }
}