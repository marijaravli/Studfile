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
            if (User.IsInRole("Profesor"))
            {
                return DohvatiSveProfesoroveZakazaneSeminare();
            }
            else if (User.IsInRole("Student"))
            {
                return null; //  DohvatiSveStudentoveSeminare();
            }
            else
            {
                // Ako nije ni student ni profesor neka se prijavi!
                return RedirectToAction("Login", "Account");
            }
        }

        private ActionResult DohvatiSveProfesoroveZakazaneSeminare()
        {
            string id = HttpContext.User.Identity.GetUserId();
            Profesor prof = db.Profesor.Where(p => p.UserId == id).FirstOrDefault();
            if (prof == null)
            {
                return RedirectToAction("Create", "Profesor");
            }

            IEnumerable<SeminarDatumListViewModel> datumi = db.SeminarDatum
                .Join(db.Kolegij, sd => sd.KolegijId, k => k.Id, (seminarDatum, kolegij) => new { seminarDatum = seminarDatum, kolegij = kolegij })
                 .Join(db.KolegijProfesor, sdk => sdk.kolegij.Id, kp => kp.KolegijId, (sdk, kp) => new { seminarDatum = sdk.seminarDatum, profesorId = kp.ProfesorId, kolegij = sdk.kolegij })
                .Join(
                    db.TimSeminarDatumSeminars,
                    spk => spk.seminarDatum.Id,
                    tsds => tsds.VrijemeIzlaganjaId,
                    (spk, tsds) => new { seminarDatum = spk.seminarDatum, profesorId = spk.profesorId, kolegij = spk.kolegij, timId = tsds.TimId, seminarId = tsds.SeminarId }
                )
                .Join(
                    db.Tims,
                    all => all.timId,
                    t => t.Id,
                    (all, t) => new { seminarDatum = all.seminarDatum, profesorId = all.profesorId, kolegij = all.kolegij, Tim = t, seminarId = all.seminarId }
                )
                .Join(
                    db.Seminar,
                    all => all.seminarId,
                    s => s.Id,
                    (all, s) => new { seminarDatum = all.seminarDatum, profesorId = all.profesorId, kolegij = all.kolegij, Tim = all.Tim, Seminar = s }
                 )
                .Where(joinedTables => joinedTables.profesorId == prof.Id)
                .Select(x => new SeminarDatumListViewModel
                {
                    Kolegij = x.kolegij,
                    Seminar = x.Seminar,
                    SeminarDatum = x.seminarDatum,
                    Studenti = db.Student.Join(db.StudentTims, s => s.Id, st => st.StudentId, (s, st) => new { student = s, timId = st.TimId }).Where(st => st.timId == x.Tim.Id).Select(st => st.student),
                    Tim = x.Tim
                }).ToList();

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

            SeminarDatum seminarDatum = new SeminarDatum { KolegijId = Id, TerminIzlaganja = DateTime.Now };
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

