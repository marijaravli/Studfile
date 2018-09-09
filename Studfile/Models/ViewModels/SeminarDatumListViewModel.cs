using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studfile.Models.ViewModels
{
    public class SeminarDatumListViewModel
    {
        public SeminarDatum SeminarDatum { get; set; }
        public Seminar Seminar { get; set; }
        public Kolegij Kolegij { get; set; }
        public Tim Tim { get; set; }
        public IEnumerable<Student> Studenti { get; set; }
    }
}