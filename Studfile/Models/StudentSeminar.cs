using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class StudentSeminar
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SeminarId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Seminar Seminar { get; set; }
    }
}