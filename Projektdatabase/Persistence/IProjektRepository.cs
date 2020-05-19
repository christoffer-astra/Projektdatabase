using System.Collections.Generic;
using Projektdatabase.Models;

namespace Projektdatabase.Persistence
{
    public interface IProjektRepository
    {
        
        ProjektModel GetModel(int id);
        
        List<ProjektModel> GetAll(IDbConnectionFactory conn);
    }
}
