using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class KolegijViewModel
    {
        
        public int Id { get; set; }

        [Display(Name = "Naziv kolegija")]
        public string Naziv { get; set; }

        [Display (Name = "Maksimalna veličina grupe")]
        public int MaksimalnaVelicinaGrupe { get; set; }

        public int BrojStudenata { get; set; }

        public virtual ICollection<Seminar> Seminar { get; set; }
    }
}