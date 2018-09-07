using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studfile.Models.ViewModels
{
    public class SeminarViewModel
    {
       
        public Seminar seminar { get; set; }

        public IEnumerable<Seminar> seminariKolegija { get; set; }
    }
}