using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studfile.Models.ViewModels
{
    public class OdabraniKolegijViewModel
    {
        public Kolegij kolegij { get; set; }
        public Tim tim { get; set; }
        public IEnumerable<Student> clanoviTima { get; set; }
        public Seminar seminar { get; set; }
        public SeminarDatum seminarDatum { get; set; }
        public IEnumerable<SelectListItem> dostupniSeminari { get; set; }
        public IEnumerable<SelectListItem> dostupniTermini { get; set; }
        public TimSeminarDatumSeminar timSeminarDatumSeminar { get; set; }


    }
}