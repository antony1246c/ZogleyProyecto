namespace ProyectoZOGLEY.Models
{
    /// <summary>
    /// Representa la información principal de un usuario registrado en el sistema.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Identificador único del usuario dentro del sistema.
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nombre completo del usuario (nombre y apellido).
        /// </summary>
        public string? NombreApellido { get; set; }

        /// <summary>
        /// Dirección de correo electrónico del usuario. Debe ser única.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Número telefónico de contacto del usuario.
        /// </summary>
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Provincia { get; set; }
        public string? Distrito { get; set; }
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Contraseña del usuario, preferiblemente almacenada en formato encriptado.
        /// </summary>
        public string? Contrasenia { get; set; }

        /// <summary>
        /// Rol asignado al usuario dentro del sistema (por ejemplo: "Cliente" o "Administrador").
        /// </summary>
        public string? Rol { get; set; }

        /// <summary>
        /// Lista de compras realizadas por el usuario, asociada mediante relación uno a muchos.
        /// </summary>
        public ICollection<ComprasUsuarios>? Compras { get; set; }
    }
}
