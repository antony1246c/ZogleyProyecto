using Microsoft.EntityFrameworkCore;

namespace ProyectoZOGLEY.Models.DTOS
{
    [Keyless]
    public class UsuariosInformacionCompletaDto
    {
        public string NombreApellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Rol { get; set; }
    }
}