namespace Studfile.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profesors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Ime = c.String(),
                    Prezime = c.String(),
                    UserId = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Students",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Ime = c.String(nullable: false),
                    Prezime = c.String(nullable: false),
                    JMBAG = c.Int(nullable: false),
                    UserId = c.String(nullable: false)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Kolegijs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Naziv = c.Int(nullable: false),
                    MaxVelicinaGrupe = c.Int(defaultValue: 3)
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
               "dbo.Tims",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   Naziv = c.Int(nullable: false)
               })
               .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.StudentTims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    StudentId = c.Int(nullable: false),
                    TimId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Tims", t => t.TimId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.TimId);

            CreateTable(
                "dbo.KolegijProfesors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ProfesorId = c.Int(nullable: false),
                    KolegijId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profesors", t => t.ProfesorId, cascadeDelete: true)
                .ForeignKey("dbo.Kolegijs", t => t.KolegijId, cascadeDelete: true)
                .Index(t => t.ProfesorId)
                .Index(t => t.KolegijId);

            CreateTable(
                "dbo.KolegijStudents",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    StudentId = c.Int(nullable: false),
                    KolegijId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Kolegijs", t => t.KolegijId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.KolegijId);

            CreateTable(
                "dbo.SeminarDatums",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TerminIzlaganja = c.DateTime(nullable: false),
                    KolegijId = c.Int()
                }
                ).PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kolegijs", t => t.KolegijId, cascadeDelete: true)
                .Index(t => t.KolegijId);

            CreateTable(
                "dbo.Seminars",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TemaSeminara = c.String(nullable: false),
                    KolegijId = c.Int()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kolegijs", t => t.KolegijId, cascadeDelete: true)
                .Index(t => t.KolegijId);

            CreateTable(
                "dbo.TimSeminarDatumSeminars",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TimId = c.Int(nullable: false),
                    SeminarId = c.Int(nullable: false),
                    VrijemeIzlaganjaId = c.Int(nullable: false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Seminars", t => t.SeminarId, cascadeDelete: true)
                .ForeignKey("dbo.StudentTims", t => t.TimId)
                .ForeignKey("dbo.SeminarDatums", t => t.VrijemeIzlaganjaId)
                .Index(t => t.TimId)
                .Index(t => t.VrijemeIzlaganjaId)
                .Index(t => t.SeminarId);


        }
        


        public override void Down()
        {
            DropForeignKey("dbo.StudentTims", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentTims", "TimId", "dbo.Tims");

            DropForeignKey("dbo.KolegijProfesors", "ProfesorId", "dbo.Profesors");
            DropForeignKey("dbo.KolegijProfesors", "KolegijId", "dbo.Kolegijs");

            DropForeignKey("dbo.KolegijStudents", "StudentId", "dbo.Students");
            DropForeignKey("dbo.KolegijStudents", "KolegijId", "dbo.Kolegijs");

            DropForeignKey("dbo.SeminarDatums", "KolegijId", "dbo.Kolegijs");

            DropForeignKey("dbo.Seminars", "KolegijId", "dbo.Kolegijs");

            DropForeignKey("dbo.TimSeminarDatumSeminars", "SeminarId", "dbo.Seminars");
            DropForeignKey("dbo.TimSeminarDatumSeminars", "TimId", "dbo.Tims");
            DropForeignKey("dbo.TimSeminarDatumSeminars", "VrijemeIzlaganjaId", "dbo.SeminarDatums");
            
            // Dropping indexes
            DropIndex("dbo.StudentTims", new[] { "StudentId" });
            DropIndex("dbo.StudentTims", new[] { "TimId" });
            DropIndex("dbo.KolegijProfesors", new[] { "ProfesorId" });
            DropIndex("dbo.KolegijProfesors", new[] { "KolegijId" });
            DropIndex("dbo.KolegijStudents", new[] { "StudentId" });
            DropIndex("dbo.KolegijStudents", new[] { "KolegijId" });
            DropIndex("dbo.SeminarDatums", new[] { "KolegijId" });
            DropIndex("dbo.Seminars", new[] { "KolegijId" });
            DropIndex("dbo.TimSeminarDatumSeminars", new[] { "SeminarId" });
            DropIndex("dbo.TimSeminarDatumSeminars", new[] { "TimId" });
            DropIndex("dbo.TimSeminarDatumSeminars", new[] { "VrijemeIzlaganjaId" });
            
            // Dropping tables
            DropTable("dbo.Profesors");
            DropTable("dbo.Students");
            DropTable("dbo.Courses");
            DropTable("dbo.Tims");
            DropTable("dbo.StudentTims");
            DropTable("dbo.KolegijProfesors");
            DropTable("dbo.KolegijStudents");
            DropTable("dbo.SeminarDatums");
            DropTable("dbo.Seminars");
            DropTable("dbo.TimSeminarDatumSeminars");
        }
    }
}
