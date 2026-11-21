namespace ProyectoZOGLEY.Models
{
    public class Factura
    {
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Subtotal { get; set; }
        public decimal ITBMS { get; set; }
        public decimal CostoEnvio { get; set; }
        public decimal Total { get; set; }
        public string TipoEntrega { get; set; } = "recoger";
        public string? Direccion { get; set; }
        public string? Provincia { get; set; }
        public string? Distrito { get; set; }
        public string? Referencia { get; set; }
        public string? Notas { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string MetodoPago { get; set; } = "Contra entrega";
        public bool CorreoEnviado { get; set; }
        public byte[]? ArchivoPdf { get; set; }
        public UserInfo? Usuario { get; set; }
        public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
    }
}
