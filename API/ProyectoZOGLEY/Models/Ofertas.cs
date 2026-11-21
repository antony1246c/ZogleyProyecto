namespace ProyectoZOGLEY.Models
{
    public class Ofertas
    {
        public int IdOferta { get; set; }
        public int IdProducto { get; set; }
        public decimal DescuentoMonto { get; set; }
        public bool Activa { get; set; }

        public ProductosRopa? Producto { get; set; }
    }
}
