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
        [Display(Name = "Støtte Noter")]
        public string ProjektFundingDescription { get; set; }

        [Display(Name = "Start Dato"), DataType(DataType.Date)]
        public DateTime ProjektStartDate { get; set; }

        //public DateTime ProjektStartDate { get; set; }
        [Display(Name = "Slut Dato"), DataType(DataType.Date)]
        public DateTime ProjektEndDate { get; set; }
        [Display(Name = "Link")]
        public string ProjektLink { get; set; }
        [Display(Name = "Evalueringstype")]
        public string ProjektEvaluation { get; set; }
        [Display(Name = "Type")]
        public string ProjektProgression { get; set; }
        [Display(Name = "Uddannelses Område")]
        public IEnumerable<UddOmrModel> UddOmrModels { get; set; }
        //public IEnumerable<OmrModel> OmrModels { get; set; }
        [Display(Name = "Klassifikation")]
        public IEnumerable<KlassifikationModel> KlassifikationModels { get; set; }
        //public IEnumerable<ProjektHolderModel> ProjektHolderModels { get; set; }
        //public IEnumerable<DeltagendeInstModel> DeltagendeInstModels { get; set; }
    }
}