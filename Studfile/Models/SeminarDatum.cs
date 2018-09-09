using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class SeminarDatum
    {
        public int Id { get; set; }

        [Display (Name = "Termin izlaganja")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TerminIzlaganja { get; set; }

        [Display (Name = "Kolegij")]
        public int KolegijId { get; set; }
    }
}