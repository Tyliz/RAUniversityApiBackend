using System.ComponentModel.DataAnnotations;

namespace RAUniversityApiBackend.Models.DataModels
{
    public enum CourseLevel
    {
		Basic = 1,
        Medium = 2,
        Advanced = 3,
        Expert = 4,
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
        public string Requirements { get; set; } = string.Empty;

        [Required]
        public CourseLevel Level { get; set; } = CourseLevel.Basic;

        [Required]
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

		public ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<Student> Students { get; set; } = new List<Student>();
	}
}
