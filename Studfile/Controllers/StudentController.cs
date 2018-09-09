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

                IEnumerable<KolegijWithSeminarViewModel> kolegijiNaKojimaImamSeminar = db.Kolegij
                    .Join(
                        db.KolegijStudents,
                        k => k.Id,
                        ks => ks.KolegijId,
                        (k, ks) => new { kolegij = k, studentId = ks.StudentId }
                    )
                    .Where(ks => ks.studentId == student.Id)
                    .Join(
                        db.StudentTims,
                        ks => ks.studentId,
                        st => st.StudentId,
                        (ks, st) => new { kolegij = ks.kolegij, timId = st.TimId }
                    )
                    .Join(
                        db.Tims,
                        kt => kt.timId,
                        t => t.Id,
                        (kt, t) => new { kolegij = kt.kolegij, tim = t }
                    )
                    .Where(kt => kt.kolegij.Id == kt.tim.KolegijId)
                    .Join(
                        db.TimSeminarDatumSeminars,
                        kt => kt.tim.Id,
                        tsd => tsd.TimId,
                        (kt, tsd) => new { kolegij = kt.kolegij, timId = kt.tim.Id, seminarId = tsd.SeminarId }
                    )
                    .Join(
                        db.Seminar,
                        kts => kts.seminarId,
                        s => s.Id,
                        (kts, s) => new { kolegij = kts.kolegij, seminar = s }
                    )
                    .Select(x => new KolegijWithSeminarViewModel { kolegij = x.kolegij, seminar = x.seminar });



                IEnumerable<KolegijWithSeminarViewModel> kolegijiNaKojimaNemamSeminar = db.Kolegij
                    .Join(
                        db.KolegijStudents,
                        k => k.Id,
                        ks => ks.KolegijId,
                        (k, ks) => new { kolegij = k, studentId = ks.StudentId }
                    )
                    .Where(ks => ks.studentId == student.Id && !kolegijiNaKojimaImamSeminar.Select(x => x.kolegij).Contains(ks.kolegij))
                    .Select(x => new KolegijWithSeminarViewModel { kolegij = x.kolegij, seminar = null });

                IEnumerable<KolegijWithSeminarViewModel> sviMojiKolegiji = kolegijiNaKojimaImamSeminar.Union(kolegijiNaKojimaNemamSeminar);

                return View(sviMojiKolegiji);
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

        // GET: Student/OdaberiKolegij/1
        public ActionResult OdaberiKolegij(int kolegijId)
        {
            string id = HttpContext.User.Identity.GetUserId();
            Student student = db.Student.Where(s => s.UserId == id).FirstOrDefault();

            Kolegij odabraniKolegij = db.Kolegij.FirstOrDefault(k => k.Id == kolegijId);
            Tim timZaKolegij = db.Tims
                .Join(
                    db.StudentTims,
                    t => t.Id,
                    st => st.TimId,
                    (t, st) => new { tim = t, studentId = st.StudentId }
                )
                .Where(ts => ts.studentId == student.Id)
                .Select(ts => ts.tim)
                .FirstOrDefault(t => t.KolegijId == odabraniKolegij.Id);

            // Kreirajmo timSeminarDatumSeminar koji cemo slati nazad kad student sve odabere kroz formu
            TimSeminarDatumSeminar timSeminarDatumSeminar = new TimSeminarDatumSeminar();

            // Mozemo traziti clanove tek kad imamo napravljen kolegij
            // inace ce ovo dolje 'timZaKolegij.id' baciti gresku jer ne postoji timZaKolegij
            IEnumerable<Student> clanoviTima = new List<Student>();
            Seminar odabraniSeminar = null;
            SeminarDatum odabraniSeminarDatum = null;

            if (timZaKolegij != null)
            {
                timSeminarDatumSeminar.TimId = timZaKolegij.Id;

                clanoviTima = db.Student
                    .Join(
                        db.StudentTims,
                        s => s.Id,
                        st => st.StudentId,
                        (s, st) => new { student = s, timId = st.TimId }
                    )
                    .Where(st => st.timId == timZaKolegij.Id)
                    .Select(st => st.student)
                    .ToList();

                // Pronađimo Seminar ako smo već odabrali
                odabraniSeminar = db.Seminar
                    .Join(
                        db.TimSeminarDatumSeminars,
                        s => s.Id,
                        tsds => tsds.SeminarId,
                        (s, tsds) => new { seminar = s, timSeminarDatumSeminar = tsds }
                        )
                        .Where(joinedTables => joinedTables.timSeminarDatumSeminar.TimId == timZaKolegij.Id)
                        .Select(joinedTables => joinedTables.seminar)
                        .FirstOrDefault();

                // Pronađimo SeminarDatum ako smo već odabrali
                odabraniSeminarDatum = db.SeminarDatum
                    .Join(
                        db.TimSeminarDatumSeminars,
                        sd => sd.Id,
                        tsds => tsds.VrijemeIzlaganjaId,
                        (sd, tsds) => new { seminarDatum = sd, timSeminarDatumSeminar = tsds }
                        )
                        .Where(joinedTables => joinedTables.timSeminarDatumSeminar.TimId == timZaKolegij.Id)
                        .Select(joinedTables => joinedTables.seminarDatum)
                        .FirstOrDefault();
            }

            // Dohvatimo sve zauzete seminare da mozemo filtrirati ostale
            IEnumerable<Seminar> zauzetiSeminariKolegija = db.Seminar
                .Where(s => s.KolegijId == kolegijId)
                .Join(
                    db.TimSeminarDatumSeminars,
                    s => s.Id,
                    tsds => tsds.SeminarId,
                    (s, tsds) => new { seminar = s, timSeminarDatumSeminar = tsds }
                )
                .Select(joinedTables => joinedTables.seminar);

            // Dohvatimo sve slobodne seminare
            IEnumerable<SelectListItem> slobodniSeminari = null;
            if (odabraniSeminar != null)
            {
                slobodniSeminari = db.Seminar
                    .Where(s => s.KolegijId == kolegijId)
                    .Where(s => !zauzetiSeminariKolegija.Contains(s) || s.Id == odabraniSeminar.Id)
                    .Select(s => new SelectListItem { Text = s.TemaSeminara, Value = s.Id.ToString() });
            }
            else
            {
                slobodniSeminari = db.Seminar
                    .Where(s => s.KolegijId == kolegijId)
                    .Where(s => !zauzetiSeminariKolegija.Contains(s))
                    .Select(s => new SelectListItem { Text = s.TemaSeminara, Value = s.Id.ToString() });

            }


            // Dohvatimo sve zauzete datume da mozemo filtrirati ostale
            IEnumerable<SeminarDatum> zauzetiDatumiKolegija = db.SeminarDatum
                .Where(sd => sd.KolegijId == kolegijId)
                .Join(
                    db.TimSeminarDatumSeminars,
                    sd => sd.Id,
                    tsds => tsds.VrijemeIzlaganjaId,
                    (sd, tsds) => new { seminarDatum = sd, timSeminarDatumSeminar = tsds }
                )
                .Select(joinedTables => joinedTables.seminarDatum);

            // Dohvatimo sve slobodne seminare
            IEnumerable<SelectListItem> slobodniSeminarDatumi = null;

            if (odabraniSeminarDatum != null)
            {
                slobodniSeminarDatumi = db.SeminarDatum
                    .Where(sd => sd.KolegijId == kolegijId)
                    .Where(sd => !zauzetiDatumiKolegija.Contains(sd) || sd.Id == odabraniSeminarDatum.Id)
                    .Select(sd => new SelectListItem { Text = sd.TerminIzlaganja.ToString(), Value = sd.Id.ToString() });
            }
            else
            {
                slobodniSeminarDatumi = db.SeminarDatum
                    .Where(sd => sd.KolegijId == kolegijId)
                    .Where(sd => !zauzetiDatumiKolegija.Contains(sd)) // zaboravio sam to maknit...
                    .Select(sd => new SelectListItem { Text = sd.TerminIzlaganja.ToString(), Value = sd.Id.ToString() });
            }

            // Kreiranje viewModela
            OdabraniKolegijViewModel odabraniKolegijViewModel = new OdabraniKolegijViewModel
            {
                kolegij = odabraniKolegij,
                seminar = odabraniSeminar,
                seminarDatum = odabraniSeminarDatum,
                tim = timZaKolegij,
                clanoviTima = clanoviTima,
                dostupniSeminari = slobodniSeminari,
                dostupniTermini = slobodniSeminarDatumi,
                timSeminarDatumSeminar = timSeminarDatumSeminar
            };

            return View(odabraniKolegijViewModel);
        }

        [HttpPost]
        public ActionResult SpremiTimSeminarDatumSeminar([Bind(Include = "Id,TimId,SeminarId,VrijemeIzlaganjaId")] TimSeminarDatumSeminar timSeminarDatumSeminar)
        {
            if (ModelState.IsValid)
            {
                // Dohvati ako smo već prije kreirali timSeminarDatumSeminar
                TimSeminarDatumSeminar stariTimSeminarDatumSeminar = db.TimSeminarDatumSeminars.FirstOrDefault(tsds => tsds.TimId == timSeminarDatumSeminar.TimId);

                // Ako postoji stari, moramo ga obrisati
                if (stariTimSeminarDatumSeminar != null)
                {
                    db.TimSeminarDatumSeminars.Remove(stariTimSeminarDatumSeminar);
                    db.SaveChanges();
                }

                // Spremamo novi u bazu
                TimSeminarDatumSeminar noviTimSeminarDatumSeminar = db.TimSeminarDatumSeminars.Add(timSeminarDatumSeminar);
                db.SaveChanges();
            }

            // Ovo nam je potrebno da znamo na koji kolegij ga poslati nakon sto spremi podatke
            int kolegijId = db.Tims.FirstOrDefault(t => t.Id == timSeminarDatumSeminar.TimId).KolegijId;

            return RedirectToAction("OdaberiKolegij", new { kolegijId = kolegijId });
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