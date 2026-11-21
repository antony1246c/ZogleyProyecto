using Microsoft.EntityFrameworkCore;
using ProyectoZOGLEY.Data;
using ProyectoZOGLEY.Models;
using ProyectoZOGLEY.Models.DTOS;

namespace ProyectoZOGLEY.Services
{
    public class ComprasUsuarioService
    {
        private readonly ApplicationDbContext _context;
        public ComprasUsuarioService(ApplicationDbContext context) => _context = context;

        public async Task<(bool Exito, object Resultado)> RegistrarComprasAsync(ComprasUsuariosDto compra, int idUsuario)
        {
            if (string.IsNullOrWhiteSpace(compra.NombreProducto) || compra.Cantidad <= 0)
                return (false, new { mensaje = "Debe elegir un producto y una cantidad válida." });

            var usuario = await _context.DatosUsuarios.FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
            if (usuario == null) return (false, new { mensaje = "El usuario autenticado no existe." });

            var producto = await _context.Productos.Include(p => p.Inventario)
                .FirstOrDefaultAsync(p => p.NombreProducto == compra.NombreProducto && p.Activo);
            if (producto == null) return (false, new { mensaje = "El producto no existe o no está disponible." });
            if (producto.Inventario == null || producto.Inventario.Stock < compra.Cantidad)
                return (false, new { mensaje = "No hay inventario suficiente." });

            producto.Inventario.Stock -= compra.Cantidad;
            _context.ComprasUsuarios.Add(new ComprasUsuarios
            {
                IdUsuario = idUsuario,
                IdProducto = producto.IdProducto,
                Cantidad = compra.Cantidad,
                PrecioUnitario = producto.Precio ?? 0m,
                Talla = compra.Talla,
                FechaCompra = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return (true, new { mensaje = "Compra registrada exitosamente." });
        }
    }
}
