using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using School_CRUD.Data;
using School_CRUD.Models;

namespace School_CRUD.Controllers
{
    public class DegreeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DegreeController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: DegreeController
        public IActionResult Index()
        {
            IEnumerable<Degree> objectDegreeList = _db.degrees.Include(d => d.Teacher).ToList();
            return View(objectDegreeList);
        }

        // GET: DegreeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DegreeController/Create
        public IActionResult Create()
        {
            var availableTeacher = _db.Teachers
                                    .Where(t => !_db.degrees.Any(g => g.TeacherId == t.Id))
                                    .Select(t => new 
                                    { 
                                        t.Id, 
                                        Name = t.Name + ' ' + t.FirstName, 
                                    }).ToList();
            ViewBag.TeacherList = new SelectList(availableTeacher, "Id", "Name");
            return View();
        }

        // POST: DegreeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Degree degree)
        {
            if (ModelState.IsValid)
            {
                _db.degrees.Add(degree);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(degree);
        }

        // GET: DegreeController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var degree = _db.degrees.Include(d => d.Teacher).FirstOrDefault(d => d.Id == id);

            if (degree == null)
            {
                return NotFound(id);
            }

            var availableTeachers = _db.Teachers
                                    .Where(t => !_db.degrees.Any(d => d.TeacherId == t.Id) || t.Id == degree.TeacherId)
                                    .Select(t => new
                                    {
                                        t.Id,
                                        Name = t.Name + " " + t.LastName
                                    }).ToList();

            ViewBag.TeacherList = new SelectList(availableTeachers, "Id", "Name", degree.TeacherId);

            return View(degree);
        }

        // POST: DegreeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Degree degree)
        {
            if (id != degree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(degree);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DegreeExists(degree.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var availableTeachers = _db.Teachers
                                       .Where(t => !_db.degrees.Any(d => d.TeacherId == t.Id) || t.Id == degree.TeacherId)
                                       .Select(t => new
                                       {
                                           t.Id,
                                           Name = t.Name + " " + t.LastName
                                       }).ToList();

            ViewBag.TeacherList = new SelectList(availableTeachers, "Id", "Name", degree.TeacherId);
            return View(degree);
        }

        private bool DegreeExists(int id)
        {
            return _db.degrees.Any(e => e.Id == id);
        }

        // GET: DegreeController/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            var degree = _db.degrees.Include(d => d.Teacher).FirstOrDefault(d => d.Id == id);
            if (degree == null) return NotFound(id);
            return View(degree);
        }

        // POST: DegreeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Degree degree)
        {
            _db.degrees.Remove(degree);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult SeeStudents(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var degree = _db.degrees.Find(id);

            if (degree == null) return NotFound(id);

            var studentDegrees = _db.StudentDegrees
                                    .Where(s => s.DegreeId == id)
                                    .ToList();

            var studentIds = studentDegrees.Select(sd => sd.StudentId).ToList();
            var students = _db.Students
                              .Where(s => studentIds.Contains(s.Id))
                              .ToList();

            var tuple = new Tuple<Degree, List<Student>>(degree, students);

            return View(tuple);
        }

        public IActionResult AddStudentToDegree(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var studentNotDegree = _db.Students
                                   .Where(s => !_db.StudentDegrees.Select(sd => sd.StudentId).Contains(s.Id))
                                   .ToList();
            var tuple = new Tuple<int?, List<Student>>(id, studentNotDegree);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudentToDegreeConfirmed(int degreeId, int studentId, string section)
        {
            var degree = _db.degrees.Find(degreeId);
            var student = _db.Students.Find(studentId);
            if (degree == null || student == null) return NotFound();

            var studentDegree = new StudentDegree
            {
                DegreeId = degreeId,
                StudentId = studentId,
                Section = section
            };

            _db.StudentDegrees.Add(studentDegree);
            _db.SaveChanges();

            return RedirectToAction("SeeStudents", new { id = degreeId });
        }

        public IActionResult RemoveStudentToDegree(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var studentDegree = _db.StudentDegrees
                           .Where(sd => sd.StudentId == id)
                           .FirstOrDefault();

            if (studentDegree == null)
            {
                return NotFound(id);
            }

            return View(studentDegree);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveStudentFromDegreeConfirmed(int degreeId, int studentId)
        {
            var studentDegree = _db.StudentDegrees
                                   .FirstOrDefault(sd => sd.StudentId == studentId && sd.DegreeId == degreeId);

            if (studentDegree == null)
            {
                return NotFound();
            }

            _db.StudentDegrees.Remove(studentDegree);
            _db.SaveChanges();

            return RedirectToAction("SeeStudents", new { id = degreeId });
        }
    }
}
