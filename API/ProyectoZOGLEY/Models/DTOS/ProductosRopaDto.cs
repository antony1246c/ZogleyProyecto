using Microsoft.EntityFrameworkCore;

namespace ProyectoZOGLEY.Models
{
    /// <summary>
    /// DTO sin clave primaria utilizado para recibir datos desde
    /// procedimientos almacenados relacionados con productos de ropa.
    /// </summary>
    [Keyless] // Indica que esta clase no tiene clave primaria, la cual solo usamos para mapear resultados de SP
    public class ProductosRopaDto
    {
        /// <summary>
        /// Nombre del producto o prenda de ropa.
        /// </summary>
        public string NombreProducto { get; set; }

        /// <summary>
        /// Género al que pertenece el producto (Hombre, Mujer, Unisex, etc.).
        /// </summary>
        public string Genero { get; set; }

        /// <summary>
        /// Categoría del producto (camisetas, pantalones, calzado, etc.).
        /// </summary>
        public string Categoria { get; set; }

        /// <summary>
        /// Precio actual del producto.
        /// </summary>
        public decimal Precio { get; set; }

        /// <summary>
        /// Enlace a imagen del producto.
        /// </summary>
        public string? EnlaceIMG { get; set; }
    }
}
