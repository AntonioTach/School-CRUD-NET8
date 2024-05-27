using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_CRUD.Models
{
    public class StudentDegree
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        public Student Student { get; set; }

        [Required]
        [ForeignKey("Degree")]
        public int DegreeId { get; set; }
        public Degree Degree { get; set; }

        [Required]
        [MaxLength(100)]
        public string Section { get; set; }
    }
}
