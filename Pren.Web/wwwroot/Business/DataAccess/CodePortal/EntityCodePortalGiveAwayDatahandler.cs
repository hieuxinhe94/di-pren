using System.Data.Entity.Migrations;
using System.Linq;
using Pren.Web.Business.Data;
using Pren.Web.Business.DataAccess.CodePortal.Entities;

namespace Pren.Web.Business.DataAccess.CodePortal
{
    public class EntityCodePortalGiveAwayDatahandler : EntityDataHandlerBase, ICodePortalGiveAwayDatahandler
    {
        public GiveAwayEntity GetGiveAway(int codeId)
        {
            return GetEntities(dbContext =>
            {
                var giveAway = dbContext.CodePortalGiveAway
                    .FirstOrDefault(x => x.fkCodeId.Equals(codeId));

                if (giveAway == null)
                {
                    return null;
                }

                return ConvertToGiveAwayEntity(giveAway);
            });
        }

        public void AddGiveAway(GiveAwayEntity giveAway)
        {
            AddOrUpdateEntities(dbContext =>
            {
                dbContext.CodePortalGiveAway.AddOrUpdate(new CodePortalGiveAway
                {
                   fkCodeId = giveAway.CodeId,
                   GiveAwayTo = giveAway.GiveAwayTo
                });

                dbContext.SaveChanges();
            });
        }

        private GiveAwayEntity ConvertToGiveAwayEntity(CodePortalGiveAway giveAway)
        {
            return new GiveAwayEntity
            {
                Id = giveAway.Id,
                CodeId = giveAway.fkCodeId,
                GiveAwayTo = giveAway.GiveAwayTo
            };
        }
    }
}