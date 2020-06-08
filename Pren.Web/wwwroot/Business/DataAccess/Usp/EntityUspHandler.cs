using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Pren.Web.Business.Data;
using Pren.Web.Business.DataAccess.Usp.Entities;

namespace Pren.Web.Business.DataAccess.Usp
{
    public class EntityUspHandler : EntityDataHandlerBase, IUspDataHandler
    {
        public IEnumerable<UspTextEntity> GetUspTexts(int productId)
        {
            return GetEntities<IEnumerable<UspTextEntity>>(dbContext => 
                dbContext.CampaignUspText.Where(t => t.CampaignUspProductTextRelation.Any(relation => relation.fkUspProductId.Equals(productId))).Select(t => new UspTextEntity
                {
                    Id = t.Id,
                    Text = t.Text
                }).ToList());
        }

        public IEnumerable<UspProductEntity> GetUspProducts()
        {
            return GetEntities<IEnumerable<UspProductEntity>>(
                dbContext => dbContext.CampaignUspProduct.Select(t => new UspProductEntity
                {
                    Id = t.Id,
                    Text = t.Text
                }).ToList());
        }

        public void UpdateUspText(int id, string text)
        {
            AddOrUpdateEntities(dbContext =>
            {
                dbContext.CampaignUspText.AddOrUpdate(new CampaignUspText
                {
                    Id = id,
                    Text = text
                });

                dbContext.SaveChanges();                
            });
        }

        public void DeleteUspText(int id)
        {
            DeleteEntities(dbContext =>
            {
                var relationToRemove = dbContext.CampaignUspProductTextRelation.Where(t => t.fkUspTextId.Equals(id));
                var uspToRemove = dbContext.CampaignUspText.FirstOrDefault(t => t.Id.Equals(id));

                dbContext.CampaignUspProductTextRelation.RemoveRange(relationToRemove);
                dbContext.CampaignUspText.Remove(uspToRemove);

                dbContext.SaveChanges();
            });
        }

        public void AddUspText(int productId, string uspText)
        {
            AddOrUpdateEntities(dbContext =>
            {
                var uspTextEntity = dbContext.CampaignUspText.Add(new CampaignUspText
                {
                    Text = uspText,
                });

                dbContext.CampaignUspProductTextRelation.Add(new CampaignUspProductTextRelation { fkUspProductId = productId, fkUspTextId = uspTextEntity.Id });
                dbContext.SaveChanges();
            });
        }

        public void AddUspProduct(string text)
        {
            AddOrUpdateEntities(dbContext =>
            {
                dbContext.CampaignUspProduct.Add(new CampaignUspProduct
                {
                    Text = text
                });

                dbContext.SaveChanges();  
            });
        }
    }
}