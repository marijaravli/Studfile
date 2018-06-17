using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class StudentKolegijViewModel
    {
        public int StudentId { get; set; }
        public string Kolegij { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string UserId { get; set; }
        [Display(Name = "JMBAG")]
        public string JMBAG { get; set; }
    }
}