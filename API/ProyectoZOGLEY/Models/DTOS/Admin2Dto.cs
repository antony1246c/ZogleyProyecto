using Microsoft.EntityFrameworkCore;

namespace ProyectoZOGLEY.Models.DTOS
{
    [Keyless]
    public class Admin2Dto
    {
        /// <summary>
        /// Cantidad total de los procedimientos.
        /// </summary>
        public int Totales { get; set; }
    }
}
