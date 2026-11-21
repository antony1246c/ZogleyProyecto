namespace ProyectoZOGLEY.Models.DTOS
{
    public sealed class ProductoCatalogoDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal PrecioOriginal { get; set; }
        public decimal PrecioOferta { get; set; }
        public string? EnlaceIMG { get; set; }
    }
}
