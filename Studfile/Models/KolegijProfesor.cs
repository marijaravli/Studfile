using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class KolegijProfesor
    {
        public int Id { get; set; }

        public int KolegijId { get; set; }

        public int ProfesorId { get; set; }
    }
}