using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class Teacher : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public IEnumerable<Course>? Courses { get; set; }
    }
}
