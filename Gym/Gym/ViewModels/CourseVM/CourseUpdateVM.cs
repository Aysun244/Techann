namespace Gym.ViewModels.CourseVM
{
    public class CourseUpdateVM
    {
        public int Id { get; set; } 
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public DateTime Time { get; set; }
        public int TeacherId { get; set; }
    }
}
