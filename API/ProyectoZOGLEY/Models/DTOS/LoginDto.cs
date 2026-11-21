namespace ProyectoZOGLEY.Models.DTOS
{
    /// <summary>
    /// DTO utilizado para iniciar sesión mediante email y contraseña.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Correo electrónico registrado del usuario.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contraseña ingresada por el usuario para validación.
        /// </summary>
        public string Contrasenia { get; set; }
    }
}
