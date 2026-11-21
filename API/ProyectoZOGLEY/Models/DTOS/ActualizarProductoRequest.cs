namespace ProyectoZOGLEY.Models.DTOS
{
    public sealed class ActualizarProductoRequest
    {
        public string NombreProducto { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal? PrecioOriginal { get; set; }
        public decimal? PrecioOferta { get; set; }
        public string? EnlaceIMG { get; set; }
    }

    public sealed class CambiarEstadoVentaRequest
    {
        public string Estado { get; set; } = string.Empty;
    }
}
