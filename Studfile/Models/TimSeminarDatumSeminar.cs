using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class TimSeminarDatumSeminar
    {
        public int Id { get; set; }

        public int TimId { get; set; }

        [Display (Name = "Tema seminara")]
        public int SeminarId { get; set; }

        [Display(Name = "Termin izlaganja")]
        public int VrijemeIzlaganjaId { get; set; }
    }
}