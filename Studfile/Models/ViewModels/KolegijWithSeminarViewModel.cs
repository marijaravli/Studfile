using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studfile.Models.ViewModels
{
    public class KolegijWithSeminarViewModel
    {
        public Kolegij kolegij { get; set; }
        public Seminar seminar { get; set; }
        public Tim tim { get; set; }
    }
}