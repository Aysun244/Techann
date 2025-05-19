namespace Gym.ViewModels.CourseVM
{
    public class CourseCreateVM
    {   
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
        public DateTime Time { get; set; }
        public int TeacherId { get; set; }
    }
}
