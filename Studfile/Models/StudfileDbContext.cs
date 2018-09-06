﻿using System;
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
        public DbSet<SeminarDatum> SeminarDatum { get; set; }
        public DbSet<Kolegij> Kolegij { get; set; }
        public DbSet<KolegijProfesor> KolegijProfesor { get; set; }
        public DbSet<KolegijStudent> KolegijStudents { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<StudfileDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

    }
}