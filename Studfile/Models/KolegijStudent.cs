using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class KolegijStudent
    {
        public int Id { get; set; }

        [Display (Name = "Student")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "", ApplyFormatInEditMode = true)]
        public int StudentId { get; set; }

        [Display(Name = "Kolegij")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "", ApplyFormatInEditMode = true)]
        public int KolegijId { get; set; }
    }
}