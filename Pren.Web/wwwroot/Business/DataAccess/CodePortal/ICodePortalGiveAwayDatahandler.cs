using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pren.Web.Business.DataAccess.CodePortal.Entities;

namespace Pren.Web.Business.DataAccess.CodePortal
{
    public interface ICodePortalGiveAwayDatahandler
    {
        GiveAwayEntity GetGiveAway(int codeId);
        void AddGiveAway(GiveAwayEntity giveAway);
    }
}
