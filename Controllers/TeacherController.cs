using Microsoft.AspNetCore.Mvc;
using School_CRUD.Data;
using School_CRUD.Models;

namespace School_CRUD.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TeacherController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Teacher> objectTeacherList = _db.Teachers;
            return View(objectTeacherList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _db.Teachers.Add(teacher);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var teacher = _db.Teachers.Find(id);

            if (teacher == null)
            {
                return NotFound(id);
            }

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _db.Teachers.Update(teacher);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var teacher = _db.Teachers.Find(id);

            if (teacher == null)
            {
                return NotFound(id);
            }

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Teacher teacher)
        {
            _db.Teachers.Remove(teacher);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
