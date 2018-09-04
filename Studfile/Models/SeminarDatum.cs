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

        [Display (Name = "Termini izlaganja")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TerminIzlaganja { get; set; }

        public int KolegijId { get; set; }
    }
}