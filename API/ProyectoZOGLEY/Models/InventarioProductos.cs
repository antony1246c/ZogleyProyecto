namespace ProyectoZOGLEY.Models
{
    /// <summary>
    /// Clase enfocada en el inventario asociado a un producto específico, incluyendo la cantidad disponible en stock.
    /// </summary>
    public class InventarioProductos
    {
        /// <summary>
        /// Identificador único del registro de inventario.
        /// </summary>
        public int IdInventario { get; set; }

        /// <summary>
        /// Identificador del producto al cual pertenece este inventario.
        /// </summary>
        public int IdProducto { get; set; }

        /// <summary>
        /// Cantidad disponible del producto en stock.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Objeto de navegación que representa el producto asociado a este registro de inventario.
        /// </summary>
        public ProductosRopa? Producto { get; set; }
    }
}
