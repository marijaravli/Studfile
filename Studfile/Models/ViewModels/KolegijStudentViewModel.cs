using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studfile.Models.ViewModels
{
    public class KolegijStudentViewModel
    {
        public KolegijStudent kolegijStudent { get; set; }
        
        public IEnumerable<SelectListItem> student { get; set; }
        public IEnumerable<SelectListItem> kolegij { get; set; }
    }
}