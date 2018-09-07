using Studfile.Models;
using Studfile.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studfile.Controllers
{
    public class SeminarController : Controller
    {
        private StudfileDbContext db = new StudfileDbContext();


        // GET: Seminar
        public ActionResult Index()
        {

            return View(db.Seminar.ToList());
        }

        // GET: Seminar/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Seminar/CreateForKolegij
        public ActionResult CreateForKolegij(int id)
        {
            IEnumerable<Seminar> seminariTrazenogKolegija = db.Seminar
                .Where(s => s.KolegijId == id);

            Seminar seminar = new Seminar { KolegijId = id };
            SeminarViewModel seminarViewModel = new SeminarViewModel
            {
                seminar = seminar,
                seminariKolegija = seminariTrazenogKolegija.ToList()
            };

            return View(seminarViewModel);
        }

        // POST: Seminar/CreateForKolegij
        [HttpPost]
        public ActionResult CreateForKolegij([Bind(Include = "Id,TemaSeminara,KolegijId")] Seminar seminar)
        {
            if (ModelState.IsValid)
            {
                Seminar newseminar = db.Seminar.Add(seminar);
                db.SaveChanges();


                return RedirectToAction("CreateForKolegij", new { id = seminar.KolegijId });

            }

            return RedirectToAction("CreateForKolegij", new { id = seminar.KolegijId });

        }


        // GET: Seminar/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Seminar/Edit/5
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

        // GET: Seminar/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Seminar/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Seminar seminar = db.Seminar
                .Where(s => s.Id == id).First();
            db.Seminar.Remove(seminar);
            db.SaveChanges();

            return RedirectToAction("CreateForKolegij", new { id = seminar.KolegijId });
        }

    }
}
