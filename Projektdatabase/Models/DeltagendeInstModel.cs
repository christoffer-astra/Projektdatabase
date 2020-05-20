using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projektdatabase.Models
{
    public class DeltagendeInstModel
    {
        public int DeltagendeInstId { get; set; }
        [Display(Name="Deltagende Institutioner")]
        public string DeltagendeInstName { get; set; }
    }
}
