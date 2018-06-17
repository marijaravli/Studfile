using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Studfile.Models
{
    public class StudfileDbContext: DbContext
    {
        public StudfileDbContext (): base("StudfileDbKonekcija")
        {

        }

        public DbSet<Profesor> Profesor{ get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Seminar> Seminar { get; set; }
        public DbSet<StudentSeminar> StudentSeminar { get; set; }


    }
}