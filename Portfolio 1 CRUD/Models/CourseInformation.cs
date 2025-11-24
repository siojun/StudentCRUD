namespace Portfolio_1_CRUD.Models
{
    public class CourseInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
