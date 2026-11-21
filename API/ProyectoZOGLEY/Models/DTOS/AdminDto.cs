using Microsoft.EntityFrameworkCore;

namespace ProyectoZOGLEY.Models.DTOS
{
    [Keyless]
    public class AdminDto
    {

        /// <summary>
        /// Utilizado para obtener el mes de las ventas.
        /// </summary>
        public string? Mes { get; set; }
        /// <summary>
        /// Utilizado para obtener el año de las ventas.
        /// </summary>
        public int Año { get; set; }
        /// <summary>
        /// Cantidad total de los procedimientos.
        /// </summary>
        public decimal IngresosTotales { get; set; }
    }
}
