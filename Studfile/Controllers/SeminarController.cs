using Studfile.Models;
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
            var seminar = new Seminar { KolegijId = id };
            return View(seminar);
        }

        // POST: Seminar/CreateForKolegij
        [HttpPost]
        public ActionResult CreateForKolegij([Bind(Include = "Id,TemaSeminara,KolegijId")] Seminar seminar)
        {
            if (ModelState.IsValid)
            {
                Seminar newseminar = db.Seminar.Add(seminar);
                db.SaveChanges();


                return RedirectToAction("Index");
            }


            {
                return View();
            }
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
