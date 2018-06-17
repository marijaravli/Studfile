using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        [Display(Name = "Ime")]
        public string Prezime { get; set; }
        [Display(Name = "Prezime")]
        public string UserId { get; set; }
        [Display(Name = "JMBAG")]
        public string Lozinka { get; set; }
        public virtual ICollection<StudentSeminar> StudentSeminar { get; set; }
    }
}