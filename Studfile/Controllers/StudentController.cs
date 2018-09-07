using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Studfile.Models;
using Microsoft.AspNet.Identity;
using Studfile.Models.ViewModels;

namespace Studfile.Controllers
{
    public class StudentController : Controller
    {
        private StudfileDbContext db = new StudfileDbContext();

        // GET: Student
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string id = HttpContext.User.Identity.GetUserId();
                Student student = db.Student.Where(s => s.UserId == id).FirstOrDefault();
                if (student == null)
                {
                    return RedirectToAction("Create", "Student");
                }

                return View(db.Student.ToList());
            }
            return RedirectToAction("Login", "Account");

        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ime,Prezime,JMBAG")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.UserId = HttpContext.User.Identity.GetUserId();
                db.Student.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ime,Prezime,UserId,Lozinka")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Student.Find(id);
            db.Student.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Student/AddToKolegij/1
        public ActionResult AddToKolegij(int id)
        {
            IEnumerable<Student> studentiNaTrazenomKolegiju = db.Student
                  .Join(
                      db.KolegijStudents,
                      s => s.Id,
                      ks => ks.StudentId,
                      (s, ks) => new { student = s, kolegijId = ks.KolegijId }
                  )
                  .Where(studentKolegijId => studentKolegijId.kolegijId == id)
                  .Select(studentKolegijId => studentKolegijId.student);

            IEnumerable<SelectListItem> studentiNisuNaKolegiju = db.Student
                .Where(s => !studentiNaTrazenomKolegiju.Select(ss => ss.Id).Contains(s.Id))
                .ToList()
                .Select(s => new SelectListItem { Text = s.Ime + " " + s.Prezime + " (" + s.JMBAG + ")", Value = s.Id.ToString() });

            KolegijStudent kolegijStudent = new KolegijStudent { KolegijId = id };
            StudentViewModel studentViewModel = new StudentViewModel
            {
                students = studentiNisuNaKolegiju,
                kolegijStudent = kolegijStudent,
                studentiNaKolegiju = studentiNaTrazenomKolegiju.ToList()
            };
            return View(studentViewModel);
        }


        // POST: Student/AddToKolegij/1
        [Authorize(Roles = "Profesor")]

        [HttpPost]
        public ActionResult AddToKolegij([Bind(Include = "Id,StudentId,KolegijId")] KolegijStudent kolegijStudent)
        {
            if (ModelState.IsValid)
            {
                KolegijStudent newkolegijStudent = db.KolegijStudents.Add(kolegijStudent);
                db.SaveChanges();
            }
            return RedirectToAction("AddToKolegij", new { id = kolegijStudent.KolegijId });

        }

        //POST: Student/RemoveFromKolegij/1
        [HttpPost]
        public ActionResult RemoveFromKolegij(int kolegijId, int studentId)
        {
            KolegijStudent kolegijStudent = db.KolegijStudents.Where(ks => ks.KolegijId == kolegijId && ks.StudentId == studentId).First();
            db.KolegijStudents.Remove(kolegijStudent);
            db.SaveChanges();

            return RedirectToAction("AddToKolegij", new { id = kolegijId });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
