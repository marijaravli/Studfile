﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class KolegijViewModel
    {
        
        public int Id { get; set; }

        public string Naziv { get; set; }

        [Display (Name = "Maksimalna veličina grupe")]
        public int MaksimalnaVelicinaGrupe { get; set; }
    }
}