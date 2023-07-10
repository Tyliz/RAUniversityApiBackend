using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.Models.DataModels
{
    public enum CourseLevel
    {
		Basic = 1,
        Intermediate = 2,
        Advanced = 3
	}

    public class Course : BaseEntity
    {

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(280)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string LongDescription { get; set; } = string.Empty;

        [Required]
        public bool PostedObjetive { get; set; } = false;

        [Required]
        public string Objetives { get; set; } = string.Empty;

        [Required]
        public string Requirements { get; set; } = string.Empty;

        [Required]
        public CourseLevel Level { get; set; }
    }
}
