namespace ProyectoZOGLEY.Models.DTOS
{
    /// <summary>
    /// DTO utilizado para registrar un usuario nuevo en el sistema.
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string NombreApellido { get; set; }

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Número de teléfono del usuario.
        /// </summary>
        public string Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Provincia { get; set; }
        public string? Distrito { get; set; }

        /// <summary>
        /// Contraseña en texto plano que será encriptada antes de ser almacenada.
        /// </summary>
        public string Contrasenia { get; set; }
    }
}
