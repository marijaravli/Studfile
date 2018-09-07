using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class Seminar
    {
        public int Id { get; set; }

        [Display (Name = "Tema seminara")]
        public string TemaSeminara { get; set; }
        public int KolegijId { get; set; }

    }
}