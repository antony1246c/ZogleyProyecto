namespace ProyectoZOGLEY.Models.DTOS
{
    /// <summary>
    /// DTO utilizado para registrar una compra realizada por un usuario.
    /// </summary>
    public class ComprasUsuariosDto
    {
        /// <summary>
        /// Email del usuario que realiza la compra.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del producto que se está comprando.
        /// </summary>
        public string NombreProducto { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de unidades compradas del producto.
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Talla seleccionada por el usuario (S, M, L, XL...).
        /// </summary>
        public string Talla { get; set; } = string.Empty;
    }
}
