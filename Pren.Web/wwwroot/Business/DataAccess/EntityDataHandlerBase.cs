using System;
using Pren.Web.Business.Data;

namespace Pren.Web.Business.DataAccess
{
    public abstract class EntityDataHandlerBase
    {
        protected T GetEntities<T>(Func<PrenWebMiscEntities, T> get)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                return get(dbContext);
            }
        }

        protected void AddOrUpdateEntities(Action<PrenWebMiscEntities> addOrUpdate)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                addOrUpdate(dbContext);
            }
        }

        protected T AddOrUpdateEntities<T>(Func<PrenWebMiscEntities, T> addOrUpdate)
        {
            using (var dbContext = new PrenWebMiscEntities())
            {
                return addOrUpdate(dbContext);
            }
        }

        protected void DeleteEntities(Action<PrenWebMiscEntities> delete)
        {
            AddOrUpdateEntities(delete);
        }
    }
}