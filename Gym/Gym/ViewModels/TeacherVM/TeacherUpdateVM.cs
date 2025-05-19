namespace Gym.ViewModels.TeacherVM
{
    public class TeacherUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }  
    }
}
