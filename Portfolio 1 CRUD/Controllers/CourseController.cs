using Microsoft.AspNetCore.Mvc;
using Portfolio_1_CRUD.Data;
using Portfolio_1_CRUD.Models;
using Portfolio_1_CRUD.Models.ViewModels;

namespace Portfolio_1_CRUD.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index(string courseName)
        {
            ViewBag.courseName = courseName;

            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(courseName))
            {
                query = query.Where(c => c.Name.Contains(courseName));
            }

            var courses = query.Take(100).ToList();

            return View(courses);
        }


        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(CourseCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if a course with the same name already exists (case-insensitive)
            bool exists = _context.Courses.Any(c => c.Name.ToLower() == model.Name.Trim().ToLower());
            if (exists)
            {
                ModelState.AddModelError("Name", "A course with this name already exists.");
                return View(model);
            }

            var course = new CourseInformation
            {
                Name = model.Name.Trim(),
                Credits = model.Credits
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // EDIT GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.FirstOrDefault(s => s.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var vm = new CourseEditVM();
            vm.Id = course.Id.ToString();
            vm.Name = course.Name;
            vm.Credits = course.Credits;

            return View(vm);
        }

        // EDIT POST
        [HttpPost]
        public IActionResult Edit(int id, CourseEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var course = _context.Courses.FirstOrDefault(s => s.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            // Check for duplicate name, excluding current course
            bool exists = _context.Courses
                .Any(c => c.Id != id && c.Name.ToLower() == model.Name.Trim().ToLower());
            if (exists)
            {
                ModelState.AddModelError("Name", "A course with this name already exists.");
                return View(model);
            }

            course.Name = model.Name.Trim();
            course.Credits = model.Credits;

            _context.Courses.Update(course);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // DELETE GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses.FirstOrDefault(s => s.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // DELETE POST
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.FirstOrDefault(s => s.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
