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
    public class SeminarDatumController : Controller
    {
        private StudfileDbContext db = new StudfileDbContext();

        // GET: SeminarDatum
        public ActionResult Index()
        {
            string id = HttpContext.User.Identity.GetUserId();
            Profesor prof = db.Profesor.Where(p => p.UserId == id).FirstOrDefault();
            if (prof == null)
            {
                return RedirectToAction("Create", "Profesor");
            }

            IEnumerable<SeminarDatum> datumi = db.SeminarDatum
                .Join(db.Kolegij, sd => sd.KolegijId, k => k.Id, (seminarDatum, kolegij) => new { seminarDatum = seminarDatum, kolegij = kolegij })
                .Join(db.KolegijProfesor, sdk => sdk.kolegij.Id, kp => kp.KolegijId, (sdk, kp) => new { seminarDatum = sdk.seminarDatum, profesorId = kp.ProfesorId })
                .Where(joinedTables => joinedTables.profesorId == prof.Id)
                .Select(x => x.seminarDatum)
                .ToList();

            return View(datumi);
        }

        // GET: SeminarDatum/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SeminarDatum/CreateForKolegij/5
        [Authorize(Roles = "Profesor")]

        public ActionResult CreateForKolegij(int Id)
        {
            IEnumerable<SeminarDatum> datumKolegij = db.SeminarDatum
                .Where(s => s.KolegijId == Id);

            SeminarDatum seminarDatum = new SeminarDatum { KolegijId = Id, TerminIzlaganja = DateTime.Now};
            SeminarDatumViewModels seminarDatumViewModels = new SeminarDatumViewModels
            {
                seminarDatum = seminarDatum,
                seminarDatumKolegij = datumKolegij.ToList()
            };

            return View(seminarDatumViewModels);
        }

        // POST: SeminarDatum/CreateForKolegij
        [Authorize(Roles = "Profesor")]

        [HttpPost]
        public ActionResult CreateForKolegij([Bind(Include = "Id,TerminIzlaganja,KolegijId")] SeminarDatum seminarDatum)
        {
            if (ModelState.IsValid)
            {
                SeminarDatum newseminarDatum = db.SeminarDatum.Add(seminarDatum);
                db.SaveChanges();


                return RedirectToAction("CreateForKolegij", new { id = seminarDatum.KolegijId });
            }


            return RedirectToAction("CreateForKolegij", new { id = seminarDatum.KolegijId });
        }



        // GET: SeminarDatum/Create
        [Authorize(Roles = "Profesor")]

        public ActionResult Create()
        {
            string id = HttpContext.User.Identity.GetUserId();
            Profesor prof = db.Profesor.Where(p => p.UserId == id).FirstOrDefault();
            if (prof == null)
            {
                return RedirectToAction("Create", "Profesor");
            }

            IEnumerable<SelectListItem> kolegiji = db.Kolegij
                .Join(
                    db.KolegijProfesor,
                    kolegij => kolegij.Id,
                    kolegijProfesor => kolegijProfesor.KolegijId,
                    (kolegij, kolegijProfesor) => new { Kolegij = kolegij, KolegijProfesor = kolegijProfesor }
                )
                .Where(joinedTables => joinedTables.KolegijProfesor.ProfesorId == prof.Id)
                .Select(t => t.Kolegij)
                .ToList()
                .Select(k => new SelectListItem { Text = k.Naziv, Value = k.Id.ToString() });

            var seminarDatumViewModels = new SeminarDatumViewModels { kolegiji = kolegiji, seminarDatum = new SeminarDatum { TerminIzlaganja = DateTime.Now } };

            return View(seminarDatumViewModels);
        }



        // POST: SeminarDatum/Create
        [Authorize(Roles = "Profesor")]

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,TerminIzlaganja,KolegijId")] SeminarDatum seminarDatum)
        {
            if (ModelState.IsValid)
            {
                SeminarDatum newseminarDatum = db.SeminarDatum.Add(seminarDatum);
                db.SaveChanges();


                return RedirectToAction("Index");
            }


            {
                return View();
            }
        }

        // GET: SeminarDatum/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SeminarDatum/Edit/5
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

        // GET: SeminarDatum/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SeminarDatum/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            SeminarDatum seminarDatum = db.SeminarDatum
                .Where(sd => sd.Id == id).First();
            db.SeminarDatum.Remove(seminarDatum);
            db.SaveChanges();

            return RedirectToAction("CreateForKolegij", new { id = seminarDatum.KolegijId });
        }
    }
}

