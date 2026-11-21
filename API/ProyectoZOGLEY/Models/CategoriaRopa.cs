namespace ProyectoZOGLEY.Models
{
    /// <summary>
    /// Clase que representa las categoria para las diferentes prendas
    /// </summary>
    public class CategoriaRopa
    {

        /// <summary>
        /// Identificador de cada categoria.
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Nombre de la categoria.
        /// </summary>
        public string? NombreCategoria { get; set; }

        /// <summary>
        /// Lista de productos asociados a esta categoría.
        /// </summary>
        public ICollection<ProductosRopa> Productos { get; set; }

    }
}