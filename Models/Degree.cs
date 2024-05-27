using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_CRUD.Models
{
    public class Degree
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]

        public Teacher? Teacher { get; set; }
    }
}
