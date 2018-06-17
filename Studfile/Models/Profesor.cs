﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class Profesor
    {
        public int  Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string UserId{ get; set; }
        public string Lozinka { get; set; }
        public string SifraKolegija { get; set; }
        public string NazivKolegija { get; set; }

        public virtual ICollection<Seminar> Seminar { get; set; }
    }

    
}