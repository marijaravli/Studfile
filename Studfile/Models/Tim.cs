using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class Tim
    {
        public int Id { get; set; }

        public int KolegijId { get; set; }

        [Display (Name = "Naziv tima")]
        public string Naziv { get; set; }
    }
}