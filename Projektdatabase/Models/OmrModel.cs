using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projektdatabase.Models
{
    public class OmrModel
    {
        public int OmrId { get; set; }
        [Display(Name = "Område")]
        public string OmrName { get; set; }
    }
}
