using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Studfile.Models.ViewModels
{
    public class UrediTimViewModel
    {
        public Tim tim { get; set; }
        public StudentTim studentTim { get; set; }
        public IEnumerable<Student> studentiUTimu { get; set; }
        public IEnumerable<SelectListItem> ostaliStudentiUKolegiju { get; set; }
    }
}