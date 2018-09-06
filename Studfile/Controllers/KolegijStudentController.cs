using Microsoft.AspNet.Identity;
using Studfile.Models;
using Studfile.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studfile.Controllers
{
    public class KolegijStudentController : Controller
    {
        private StudfileDbContext db = new StudfileDbContext();

        // GET: KolegijStudent
        public ActionResult Index()
        {
            string id = HttpContext.User.Identity.GetUserId();
            Profesor prof = db.Profesor.Where(p => p.UserId == id).FirstOrDefault();
            if (prof == null)
            {
                return RedirectToAction("Create", "Profesor");
            }

            IEnumerable<KolegijStudent> studenti = db.KolegijStudents
                .Join(db.Kolegij, ks => ks.KolegijId, k => k.Id, (kolegijStudent, kolegiji) => new { kolegijStudent = kolegijStudent, kolegij = kolegiji })
                .Join(db.KolegijProfesor, kpk => kpk.kolegij.Id, kp => kp.KolegijId, (kpk, kp) => new { kolegijStudent = kpk.kolegijStudent, profesorId = kp.ProfesorId })
                .Where(joinedTables => joinedTables.profesorId == prof.Id)
                .Select(y => y.kolegijStudent)
                .ToList();

            return View(studenti);
        }

        // GET: KolegijStudent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KolegijStudent/Create
        [Authorize(Roles = "Profesor")]

        public ActionResult Create()
        {
            string id = HttpContext.User.Identity.GetUserId();
            Profesor prof = db.Profesor.Where(p => p.UserId == id).FirstOrDefault();
            if (prof == null)
            {
                return RedirectToAction("Create", "Profesor");
            }

            IEnumerable<SelectListItem> kolegij = db.Kolegij
                .Join(
                    db.KolegijProfesor,
                    kolegiji => kolegiji.Id,
                    kolegijProfesor => kolegijProfesor.KolegijId,
                    (kolegiji, kolegijProfesor) => new { Kolegij = kolegiji, KolegijProfesor = kolegijProfesor }
                 )
                 .Where(joinedTables => joinedTables.KolegijProfesor.ProfesorId == prof.Id)
                 .Select(t => t.Kolegij)
                 .ToList()
                 .Select(k => new SelectListItem { Text = k.Naziv, Value = k.Id.ToString() });

            /*IEnumerable<SelectListItem> studenti = db.Student
                .Join(
                    )*/

            var kolegijStudentViewModels = new KolegijStudentViewModel { kolegij = kolegij, kolegijStudent = new KolegijStudent { } };

            return View(kolegijStudentViewModels);
        }


        // POST: KolegijStudent/Create
        [Authorize(Roles = "Profesor")]

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,StudentId,KolegijId")] KolegijStudent kolegijStudent)
        {
            if (ModelState.IsValid)
            {
                KolegijStudent newkolegijStudent = db.KolegijStudents.Add(kolegijStudent);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            {
                return View();
            }
        }

        // GET: KolegijStudent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KolegijStudent/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: KolegijStudent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KolegijStudent/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
