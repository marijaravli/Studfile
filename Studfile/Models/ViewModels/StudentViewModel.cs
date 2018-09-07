using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studfile.Models.ViewModels
{
    public class StudentViewModel
    {
        public IEnumerable<SelectListItem> students { get; set; }
        public KolegijStudent kolegijStudent { get; set; }

        public IEnumerable<Student> studentiNaKolegiju { get; set; }

    }
}