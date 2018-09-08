using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studfile.Models.ViewModels
{
    public class SeminarDatumViewModels
    {
        public SeminarDatum seminarDatum { get; set; }

        public IEnumerable<SeminarDatum> seminarDatumKolegij { get; set; }

        public IEnumerable<SelectListItem> kolegiji { get; set; }
    }
}