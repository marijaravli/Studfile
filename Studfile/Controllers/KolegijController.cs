using Microsoft.AspNet.Identity;
using Studfile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studfile.Controllers
{
    public class KolegijController : Controller
    {
        private StudfileDbContext db = new StudfileDbContext();

        private ProfesorController profesorController = new ProfesorController();

        // GET: Kolegij
        public ActionResult Index()
        {
            return View();
        }

        // GET: Kolegij/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Kolegij/Create
        [Authorize(Roles ="Profesor")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kolegij/Create
        [Authorize(Roles ="Profesor")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Naziv, MaxVelicinaGrupe")] Kolegij kolegij)
        {
            if (ModelState.IsValid)
            {
                Kolegij newKolegij = db.Kolegij.Add(kolegij);
                db.SaveChanges();
                string userId = HttpContext.User.Identity.GetUserId();
                int profesorId = db.Profesor.First(p => p.UserId == userId).Id;
                KolegijProfesor kp = new KolegijProfesor { KolegijId = newKolegij.Id, ProfesorId = profesorId };
                db.KolegijProfesor.Add(kp);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Profesor");
        }

        // GET: Kolegij/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Kolegij/Edit/5
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
        
        // POST: Kolegij/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Kolegij kolegijZaObrisat = db.Kolegij.FirstOrDefault(k => k.Id == id); 
                if (kolegijZaObrisat != null)
                {
                    db.Kolegij.Remove(kolegijZaObrisat);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Profesor");
            }
            catch
            {
                return RedirectToAction("Index", "Profesor");
            }
        }
    }
}
