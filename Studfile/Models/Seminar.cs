using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class Seminar
    {
        public int Id { get; set; }
        public string TemaSeminara { get; set; }
        public int KolegijId { get; set; }

    }
}