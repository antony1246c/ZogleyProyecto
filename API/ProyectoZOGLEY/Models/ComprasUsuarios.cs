 namespace ProyectoZOGLEY.Models
{
    /// <summary>
    /// Clase que representa el registro de una compra realizada por un usuario, incluyendo producto, cantidad y precio.
    /// </summary>
    public class ComprasUsuarios
    {
        /// <summary>
        /// Identificador único de la compra realizada.
        /// </summary>
        public int IdCompra { get; set; }

        /// <summary>
        /// Identificador del usuario que realizó la compra.
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Identificador del producto adquirido por el usuario.
        /// </summary>
        public int IdProducto { get; set; }

        /// <summary>
        /// Cantidad de unidades del producto compradas.
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio por unidad del producto al momento de realizar la compra.
        /// </summary>
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Talla del producto (por ejemplo: S, M, L, XL).
        /// </summary>
        public string? Talla { get; set; }

        /// <summary>
        /// Fecha en que se realizó la compra. Por defecto se asigna la fecha y hora actual.
        /// </summary>
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        /// <summary>
        /// Objeto de navegación que representa al usuario asociado a la compra.
        /// </summary>
        public UserInfo? Usuario { get; set; }

        /// <summary>
        /// Objeto de navegación que representa el producto adquirido.
        /// </summary>
        public ProductosRopa? Producto { get; set; }
    }
}

