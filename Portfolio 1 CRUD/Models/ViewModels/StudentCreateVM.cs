using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portfolio_1_CRUD.Models.ViewModels
{
    public class StudentCreateVM
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public List<int> SelectedCourseIds { get; set; } = new List<int>();
        public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
    }
}
