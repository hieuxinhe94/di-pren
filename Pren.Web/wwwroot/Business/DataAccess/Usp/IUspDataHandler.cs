using System.Collections.Generic;
using Pren.Web.Business.DataAccess.Usp.Entities;

namespace Pren.Web.Business.DataAccess.Usp
{
    public interface IUspDataHandler
    {        
        IEnumerable<UspTextEntity> GetUspTexts(int productId);

        IEnumerable<UspProductEntity> GetUspProducts();

        void UpdateUspText(int id, string text);

        void DeleteUspText(int id);

        void AddUspText(int productId, string uspText);
        
        void AddUspProduct(string text);
    }
}