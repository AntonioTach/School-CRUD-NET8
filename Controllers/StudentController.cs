using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using School_CRUD.Data;
using School_CRUD.Models;

namespace School_CRUD.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public StudentController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: StudentController
        public IActionResult Index()
        {
            IEnumerable<Student> objectStudentList = _db.Students;
            return View(objectStudentList);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Students.Add(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: StudentController/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0) return NotFound();

            var student = _db.Students.Find(id);

            if (student == null)
            {
                return NotFound(id);
            }

            return View(student);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Students.Update(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: StudentController/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0) return NotFound();

            var student = _db.Students.Find(id);

            if (student == null)
            {
                return NotFound(id);
            }

            return View(student);
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Student student)
        {
            _db.Students.Remove(student);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
