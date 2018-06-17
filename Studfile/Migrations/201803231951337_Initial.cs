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
                        UserId = c.String(),
                        Lozinka = c.String(),
                        SifraKolegija = c.String(),
                        NazivKolegija = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Seminars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TemaSeminara = c.String(),
                        TerminIzlaganja = c.DateTime(nullable: false),
                        ProfesorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profesors", t => t.ProfesorId, cascadeDelete: true)
                .Index(t => t.ProfesorId);
            
            CreateTable(
                "dbo.StudentSeminars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        SeminarId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Seminars", t => t.SeminarId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.SeminarId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ime = c.String(),
                        Prezime = c.String(),
                        UserId = c.String(),
                        Lozinka = c.String(),
                     })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentSeminars", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentSeminars", "SeminarId", "dbo.Seminars");
            DropForeignKey("dbo.Seminars", "ProfesorId", "dbo.Profesors");
            DropIndex("dbo.StudentSeminars", new[] { "SeminarId" });
            DropIndex("dbo.StudentSeminars", new[] { "StudentId" });
            DropIndex("dbo.Seminars", new[] { "ProfesorId" });
            DropTable("dbo.Students");
            DropTable("dbo.StudentSeminars");
            DropTable("dbo.Seminars");
            DropTable("dbo.Profesors");
        }
    }
}
