using System.ComponentModel.DataAnnotations;

namespace Techan.ViewModels.Sliders
{
    public class SliderUpdateVM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        [MinLength(16)]
        public string LittleTitle { get; set; }
        [MinLength(16)]
        public string Title { get; set; }
        [MinLength(16)]
        public string BigTitle { get; set; }
        [MinLength(16)]

        public string Offer { get; set; }
        [MinLength(16)]
        public string Link { get; set; }
    }
}
