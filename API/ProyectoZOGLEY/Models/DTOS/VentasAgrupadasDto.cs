using Microsoft.EntityFrameworkCore;

namespace ProyectoZOGLEY.Models.DTOS
{
    [Keyless]
    public class VentasAgrupadasDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }

        public string Genero { get; set; }
        public int CantidadTotal { get; set; }
        public decimal PrecioIndividual { get; set; }
        public decimal TotalGenerado { get; set; }
    }
}