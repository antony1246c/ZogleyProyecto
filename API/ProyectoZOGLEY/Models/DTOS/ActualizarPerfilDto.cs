
using System.ComponentModel.DataAnnotations;

namespace ProyectoZOGLEY.Models.DTOS
{
    public class ActualizarPerfilDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150)]
        public string NombreApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(30)]
        public string? Telefono { get; set; }

        [StringLength(250)]
        public string? Direccion { get; set; }

        [StringLength(100)]
        public string? Distrito { get; set; }

        [StringLength(100)]
        public string? Provincia { get; set; }
    }
}
