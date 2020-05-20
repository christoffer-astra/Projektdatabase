using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projektdatabase.Models
{
    public class ProjektHolderModel
    {
        public int ProjektHolderId { get; set; }
        [Display(Name="Projektholder")]
        public string ProjektHolderName { get; set; }
    }
}
