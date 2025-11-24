using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfolio_1_CRUD.Data;
using Portfolio_1_CRUD.Models;
using Portfolio_1_CRUD.Models.ViewModels;

namespace Portfolio_1_CRUD.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST with pagination and search
        public IActionResult Index(string studentName, int page = 1, int pageSize = 10)
        {
            ViewBag.studentName = studentName;
            ViewBag.CurrentPage = page;

            var query = _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .AsQueryable();

            if (!string.IsNullOrEmpty(studentName))
            {
                query = query.Where(s => s.Name.Contains(studentName));
            }

            var totalItems = query.Count();

            var students = query
                .OrderBy(s => s.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(students);
        }




        // CREATE GET
        public IActionResult Create()
        {
            // Populate courses into ViewBag
            ViewBag.Courses = _context.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(StudentCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate ViewBag.Courses if model state is invalid
                ViewBag.Courses = _context.Courses
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(model);
            }

            var student = new StudentInformation
            {
                Name = model.Name,
                Age = model.Age
            };

            if (model.SelectedCourseIds != null)
            {
                foreach (var courseId in model.SelectedCourseIds)
                {
                    student.StudentCourses.Add(new StudentCourse
                    {
                        CourseId = courseId
                    });
                }
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // EDIT GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _context.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefault(s => s.Id == id);

            if (student == null) return NotFound();

            // Populate ViewBag.Courses
            ViewBag.Courses = _context.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            var vm = new StudentEditVM
            {
                Id = student.Id.ToString(),
                Name = student.Name,
                Age = student.Age,
                SelectedCourseIds = student.StudentCourses.Select(sc => sc.CourseId).ToList()
            };

            return View(vm);
        }


        // EDIT POST
        [HttpPost]
        public IActionResult Edit(int id, StudentEditVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _context.Courses
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(model);
            }

            var student = _context.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefault(s => s.Id == id);

            if (student == null) return NotFound();

            student.Name = model.Name;
            student.Age = model.Age;

            _context.StudentCourses.RemoveRange(student.StudentCourses);

            foreach (var courseId in model.SelectedCourseIds)
            {
                student.StudentCourses.Add(new StudentCourse
                {
                    CourseId = courseId
                });
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }



        // DELETE GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        // DELETE POST
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefault(s => s.Id == id);

            if (student == null) return NotFound();

            _context.StudentCourses.RemoveRange(student.StudentCourses);
            _context.Students.Remove(student);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}