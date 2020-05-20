using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projektdatabase.Models;

namespace Projektdatabase.Persistence
{
    public interface IUnitOfWork
    {
        List<UddOmrModel> Complete();
        IEnumerable<ProjektModel> RetrieveAllProjekts();
        ProjektModel GetProjektModel();

        void SubmitProjekt(ProjektModel projektModel);
    }

}
