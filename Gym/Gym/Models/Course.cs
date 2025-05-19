namespace Gym.Models
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime Time { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
//Id get ve update saxlayir