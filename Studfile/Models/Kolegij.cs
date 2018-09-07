using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class Kolegij
    {
        public int Id { get; set; }
        
        [Display(Name = "Naziv kolegija" )]
        public string Naziv { get; set; }

        [Display(Name = "Maksimalna veličina grupe")]
        public int MaxVelicinaGrupe { get; set; }

        [ForeignKey("KolegijId")]
        public virtual ICollection<Seminar> Seminar { get; set; }
    }
}