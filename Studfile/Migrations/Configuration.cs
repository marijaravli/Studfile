namespace Studfile.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Studfile.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Studfile.Models.StudfileDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Studfile.Models.StudfileDbContext context)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(applicationDbContext));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDbContext));

            // Student role 
            if (!roleManager.RoleExists("Student"))
            {
                var role = new IdentityRole();
                role.Name = "Student";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "marijaravli@gmail.com";
                user.Email = "marijaravli@gmail.com";

                string userPWD = "RpaEfos2018$";

                var chkUser = UserManager.Create(user, userPWD);

                //Add marijaravli@gmail.com to Role Student
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Student");

                }

            }

            // Profesor role 
            if (!roleManager.RoleExists("Profesor"))
            {
                var role = new IdentityRole();
                role.Name = "Profesor";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "smitrovi@efos.hr";
                user.Email = "smitrovi@efos.hr";

                string userPWD = "RpaEfos2018$";

                var chkUser = UserManager.Create(user, userPWD);

                //Add smitrovi@efos.hr to Role Profesor
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Profesor");

                }

            }


        }
    }
}
