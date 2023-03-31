using System.ComponentModel.DataAnnotations;

namespace FinalProjectMVC.ViewModels
{
    public class EditReviewViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, 5)]
        public int Rating { get; set;}
    }
}
