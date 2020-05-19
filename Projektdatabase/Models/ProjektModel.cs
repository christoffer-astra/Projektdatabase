using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projektdatabase.Models
{
    public class ProjektModel
    {
        public int ProjektId { get; set; }
        [Display(Name = "Projekt")]
        public string ProjektName { get; set; }
        [Display(Name = "Beskrivelse")]
        public string ProjektDescription { get; set; }
        [Display(Name = "Igangværende")]
        public bool ProjektStatus { get; set; }
        [Display(Name = "Eksternt Evalueret")]
        public bool ProjektEvaluationStatus { get; set; }
        [Display(Name = "Fond Noter")]
        public string ProjektFundingDescription { get; set; }

        [Display(Name = "Start Dato"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProjektStartDate { get; set; }
        [Display(Name = "Slut Dato"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime ProjektEndDate { get; set; }
        [Display(Name = "Link")]
        public string ProjektLink { get; set; }
        [Display(Name = "Evalueringstype")]
        public string ProjektEvaluation { get; set; }
        [Display(Name = "Type")]
        public string ProjektProgression { get; set; }
        [Display(Name = "Uddannelses Område")]
        public IList<UddOmrModel> UddOmrModels { get; set; }
        [Display(Name = "Område")]
        public IList<OmrModel> OmrModels { get; set; }
        [Display(Name = "Klassifikation"), Required]
        public IList<KlassifikationModel> KlassifikationModels { get; set; }
        [Display(Name = "Projektholder")]
        public IList<ProjektHolderModel> ProjektHolderModels { get; set; }
        [Display(Name = "Deltagende Institutioner")]
        public IList<DeltagendeInstModel> DeltagendeInstModels { get; set; }
    }
}