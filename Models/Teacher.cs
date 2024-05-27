using School_CRUD.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace School_CRUD.Models
{
    public class Teacher
    {
        //Profesor: Id, Nombre Apellidos, Género
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }

        public Genre Genre { get; set; }
    }
}
