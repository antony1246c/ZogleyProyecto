namespace ProyectoZOGLEY.Models.DTOS
{
    public sealed class CrearFacturaRequest
    {
        public string TipoEntrega { get; set; } = "recoger";
        public string? Notas { get; set; }
        public string MetodoPago { get; set; } = "Contra entrega";
        public DireccionFacturaRequest? Direccion { get; set; }
        public List<DetalleFacturaRequest> Detalles { get; set; } = new();
    }

    public sealed class DetalleFacturaRequest
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string? Talla { get; set; }
    }

    public sealed class DireccionFacturaRequest
    {
        public string Direccion { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Distrito { get; set; } = string.Empty;
        public string? Referencia { get; set; }
    }
}
