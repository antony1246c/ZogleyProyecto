namespace ProyectoZOGLEY.Models
{
    /// <summary>
    /// Representa un producto de ropa dentro del catÃ¡logo de la tienda.
    /// </summary>
    public class ProductosRopa
    {
        /// <summary>
        /// Identificador Ãºnico del producto.
        /// </summary>
        public int IdProducto { get; set; }

        /// <summary>
        /// Nombre del producto de ropa.
        /// </summary>
        public string? NombreProducto { get; set; }

        /// <summary>
        /// GÃ©nero al que estÃ¡ dirigido el producto.
        /// </summary>
        public string? Genero { get; set; }


        /// <summary>
        /// Clave forÃ¡nea a la categorÃ­a del producto.
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Precio actual del producto.
        /// </summary>
        public decimal? Precio { get; set; }

        /// <summary>
        /// Enlace a imagen del producto.
        /// </summary>
        public string? EnlaceIMG { get; set; }
        /// <summary>
        /// Fecha en la que el producto fue agregado al sistema.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        public bool Activo { get; set; } = true;

        /// <summary>
        /// Informacion sobre la categorÃ­a del producto.
        /// </summary>
        public CategoriaRopa? Categoria { get; set; }

        /// <summary>
        /// InformaciÃ³n del inventario asociada al producto (stock disponible).
        /// </summary>
        public InventarioProductos? Inventario { get; set; }

        /// <summary>
        /// Lista de compras en las que aparece este producto.
        /// </summary>
        public ICollection<ComprasUsuarios>? Compras { get; set; }

        /// <summary>
        /// Lista de ofertas en las que aparece este producto.
        /// </summary>
        public ICollection<Ofertas>? Ofertas { get; set; }

    }
}
