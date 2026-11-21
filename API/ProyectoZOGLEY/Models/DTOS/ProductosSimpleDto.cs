using Microsoft.EntityFrameworkCore;

namespace ProyectoZOGLEY.Models.DTOS
{
    [Keyless]
    public class ProductosSimpleDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Genero { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public string EnlaceIMG { get; set; }

    }
}


