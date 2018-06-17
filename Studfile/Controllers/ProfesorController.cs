using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Studfile.Models;

namespace Studfile.Controllers
{
    [Authorize(Roles = "Profesor")]
    public class ProfesorController : Controller
    {
        private StudfileDbContext db = new StudfileDbContext();

        // GET: Profesor
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Profesor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesor.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // GET: Profesor/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnosStudenata(string kolegij)
        {
            var model = new UnosStudentaViewModel() { Kolegij = kolegij };
            return View(model);
        }


        // POST: Profesor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ime,Prezime,KorisnickoIme,Lozinka")] Profesor profesor)
        {
            if (ModelState.IsValid)
            {
                db.Profesor.Add(profesor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profesor);
        }

        // GET: Profesor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesor.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // POST: Profesor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ime,Prezime,KorisnickoIme,Lozinka")] Profesor profesor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profesor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profesor);
        }

        // GET: Profesor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesor.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // POST: Profesor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profesor profesor = db.Profesor.Find(id);
            db.Profesor.Remove(profesor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Profesor/TablicaStudenti
        public ActionResult TablicaStudenti()
        {
            var model = db.StudentSeminar
                .Include(s => s.Student)
                .Include(r => r.Seminar)
                .Select(x => new StudentKolegijViewModel() { Ime = x.Student.Ime, Prezime = x.Student.Prezime, JMBAG = x.Student.Lozinka, Kolegij = x.Seminar.TemaSeminara, StudentId = x.Student.Id }  );
            return View(model);
        }



        // GET: Profesor/UnosTema
        public ActionResult UnosTemai()
        {
            return View();
        }




        // GET: Profesor/UnosTerminClanovi
        public ActionResult TerminClanovi()
        {
            return View();
        }

    }
}
