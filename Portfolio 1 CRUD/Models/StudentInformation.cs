namespace Portfolio_1_CRUD.Models
{
    public class StudentInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        // Navigation property for many-to-many
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
