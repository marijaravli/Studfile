using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class Seminar
    {
        public int Id { get; set; }
        public string  TemaSeminara { get; set; }
        public DateTime TerminIzlaganja { get; set; }
        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }
        public virtual ICollection<StudentSeminar> StudentSeminar { get; set; }
    }
}