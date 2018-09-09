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
    public class TimController : Controller
    {
        private StudfileDbContext db = new StudfileDbContext();

        // GET: Tim/UrediTim/3
        public ActionResult UrediTim(int kolegijId)
        {
            string id = HttpContext.User.Identity.GetUserId();
            Student student = db.Student.Where(s => s.UserId == id).FirstOrDefault();

            Tim tim = db.Tims
                .Join(
                    db.StudentTims,
                    t => t.Id,
                    st => st.TimId,
                    (t, st) => new { tim = t, studentId = st.StudentId }
                )
                .Where(ts => ts.studentId == student.Id)
                .Select(ts => ts.tim)
                .FirstOrDefault(t => t.KolegijId == kolegijId);

            if (tim == null)
            {
                return RedirectToAction("Create", new { kolegijId = kolegijId });
            }
            IEnumerable<Student> studentiUTimu = db.Student
                .Join(
                    db.StudentTims,
                    s => s.Id,
                    st => st.StudentId,
                    (s, st) => new { student = s, timId = st.TimId }
                )
                .Where(st => st.timId == tim.Id)
                .Select(st => st.student);

            IEnumerable<Student> studentiUDrugimTimovima = db.Student
                .Join(
                    db.StudentTims,
                    s => s.Id,
                    st => st.StudentId,
                    (s, st) => new { student = s, timId = st.TimId }
                )
                .Where(st => st.timId != tim.Id)
                .Select(st => st.student);

            IEnumerable<SelectListItem> ostaliStudentiUKolegiju = db.Student
                .Join(
                    db.KolegijStudents,
                    s => s.Id,
                    ks => ks.StudentId,
                    (s, ks) => new { student = s, kolegijId = ks.KolegijId }
                )
                .Where(sk => sk.kolegijId == kolegijId)
                .Where(sk => !studentiUTimu.Contains(sk.student) && !studentiUDrugimTimovima.Contains(sk.student))
                .Select(sk => new SelectListItem { Text = sk.student.Ime + " " + sk.student.Prezime, Value = sk.student.Id.ToString() });

            UrediTimViewModel urediTimViewModel = new UrediTimViewModel
            {
                tim = tim,
                studentTim = new StudentTim { TimId = tim.Id },
                studentiUTimu = studentiUTimu.ToList(),
                ostaliStudentiUKolegiju = ostaliStudentiUKolegiju.ToList()
            };

            return View(urediTimViewModel);
        }

        // GET: Tim/Create
        public ActionResult Create(int kolegijId)
        {
            Tim tim = new Tim { KolegijId = kolegijId };

            return View(tim);
        }

        // GET: Tim/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Naziv,KolegijId")] Tim tim)
        {
            if (ModelState.IsValid)
            {
                string id = HttpContext.User.Identity.GetUserId();
                Student student = db.Student.Where(s => s.UserId == id).FirstOrDefault();

                Tim newTim = db.Tims.Add(tim);
                db.SaveChanges();
                StudentTim newStudentTim = new StudentTim { StudentId = student.Id, TimId = newTim.Id };
                db.StudentTims.Add(newStudentTim);

                db.SaveChanges();
            }

            return RedirectToAction("UrediTim", new { kolegijId = tim.KolegijId });
        }


        // POST: Tim/DodajStudentaUTim
        [HttpPost]
        public ActionResult DodajStudentaUTim([Bind(Include = "Id,StudentId,TimId")] StudentTim studentTim)
        {
            Tim tim = db.Tims.FirstOrDefault(t => t.Id == studentTim.TimId);
            Kolegij kolegij = db.Kolegij.FirstOrDefault(k => k.Id == tim.KolegijId);
            if (ModelState.IsValid)
            {
                int brojStudenataUTimu = db.StudentTims.Where(st => st.TimId == tim.Id).Count();
                if (brojStudenataUTimu + 1 <= kolegij.MaxVelicinaGrupe)
                {
                    StudentTim newStudentTime = db.StudentTims.Add(studentTim);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("UrediTim", new { kolegijId = tim.KolegijId });
        }

        // Tim/MakniStudentaIzTima
        [HttpPost]
        public ActionResult MakniStudentaIzTima(int studentId, int timId, int kolegijId)
        {
            StudentTim studentTim = db.StudentTims.FirstOrDefault(st => st.StudentId == studentId && st.TimId == timId);
            db.StudentTims.Remove(studentTim);
            db.SaveChanges();


            int ostaloStudenata = db.StudentTims.Select(st => st.TimId == timId).Count();
            if (ostaloStudenata == 0)
            {
                Tim timZaBrisat = db.Tims.FirstOrDefault(t => t.Id == timId);
                db.Tims.Remove(timZaBrisat);
                db.SaveChanges();


                TimSeminarDatumSeminar timSeminarDatumSeminar = db.TimSeminarDatumSeminars.FirstOrDefault(tsds => tsds.TimId == timId);
                db.TimSeminarDatumSeminars.Remove(timSeminarDatumSeminar);
                db.SaveChanges();
            }

            return RedirectToAction("UrediTim", new { kolegijId = kolegijId });
        }
    }
}