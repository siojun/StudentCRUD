namespace Portfolio_1_CRUD.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public StudentInformation Student { get; set; }

        public int CourseId { get; set; }
        public CourseInformation Course { get; set; }
    }
}
